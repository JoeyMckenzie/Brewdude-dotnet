using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Application.Brewery.Commands.CreateBrewery;
using Brewdude.Application.Brewery.Commands.DeleteBrewery;
using Brewdude.Application.Brewery.Commands.UpdateBrewery;
using Brewdude.Application.Brewery.Queries.GetAllBreweries;
using Brewdude.Application.Brewery.Queries.GetBreweryById;
using Brewdude.Application.Infrastructure;
using Brewdude.Application.Infrastructure.AutoMapper;
using Brewdude.Application.Security;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Brewdude.Application.UserBeers.GetBeersByUserId;
using Brewdude.Common;
using Brewdude.Domain.Entities;
using Brewdude.Infrastructure;
using Brewdude.Jwt.Services;
using Brewdude.Persistence;
using Brewdude.Web.Infrastructure;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace Brewdude.Web
{
    public class Startup
    {
        private string _jwtSecret;
        private readonly ILogger<Startup> _logger;
        
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _jwtSecret = Configuration["Brewdude:JwtSecret"];
            var jwtSigningSecret = Encoding.ASCII.GetBytes(_jwtSecret);
            
            // Register query handler assemblies
            var assemblies = new[]
            {
                typeof(GetAllBeersQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBeerByIdQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBeersByUserIdQueryHandler).GetTypeInfo().Assembly,
                typeof(CreateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(GetAllBreweriesQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBreweryByIdQueryHandler).GetTypeInfo().Assembly,
                typeof(CreateUserCommandHandler).GetTypeInfo().Assembly,
                typeof(CreateBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteBreweryCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByIdCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByUsernameCommandHandler).GetTypeInfo().Assembly
            };

            // Add services
            services.AddAutoMapper(typeof(MappingProfile).GetTypeInfo().Assembly);
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient<ITokenService>(_ => new TokenService(_jwtSecret));
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
                loggingBuilder.AddSeq(Configuration.GetSection("Seq"));
            });
            
            // Add EF Core
            services.AddDbContext<BrewdudeDbContext>(options =>
                options.UseSqlServer(Configuration["Brewdude:ConnectionString"]));

            services.AddDbContext<BrewdudeIdentityContext>(options =>
                options.UseSqlServer(Configuration["Brewdude:ConnectionString"]));

            // Add Identity
            services.AddDefaultIdentity<BrewdudeUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BrewdudeIdentityContext>()
                .AddDefaultTokenProviders();
            
            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddMediatR(assemblies);
            
            // Add JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userContext = context.HttpContext.RequestServices.GetRequiredService<BrewdudeIdentityContext>();
                        var userIdParsedSuccessfully = int.TryParse(context.Principal.Identity.Name, out var userId);
                        if (!userIdParsedSuccessfully)
                        {
                            _logger.LogWarning($"User ID was not parsed successfully for user {context.Principal.Identity.Name}");
                        }
                        else
                        {
                            var user = userContext.Users.Find(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                _logger.LogError($"User with userId [{userId}] no longer exists");
                                context.Fail("Unauthorized");
                            }  
                        }
                        return Task.CompletedTask;
                    }
                };
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSigningSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BrewdudeUserPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("User", "Admin");
                    policyBuilder.RequireClaim("username");
                    policyBuilder.RequireClaim("scopes", "read:brewery", "read:brewery");
                });
                options.AddPolicy("BrewdudeAdminPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Admin");
                    policyBuilder.RequireClaim("username");
                    policyBuilder.RequireClaim("scopes", "read:brewery", "read:brewery", "write:beer", "write:brewery");
                });
            });

            // ASP.NET Core dependencies and Fluent Validators
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    // Only need to register one validators, as they all stem from the same assembly
                    fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
                });
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                        if (error != null)
                        {
                            context.Response.ContentType = "application/json";
                            var errorResponse = new
                            {
                                ErrorId = Guid.NewGuid(),
                                TimeStamp = DateTime.UtcNow,
                                Message = error.Message
                            };
                            
                            using (var writer = new StreamWriter(context.Response.Body))
                            {
                                new JsonSerializer().Serialize(writer, errorResponse);
                                await writer.FlushAsync().ConfigureAwait(false);
                            }
                        }
                    });
                });
            }

            CreateRoles(serviceProvider).Wait();
            
            app.UseCors(o => o
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseErrorHandlingMiddleware();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        /// <summary>
        /// Seed the user roles on application startup from the roles in the domain model.
        /// </summary>
        /// <param name="serviceProvider">Scoped access provided by the DI container</param>
        /// <returns>Completed task</returns>
        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { Role.User.ToString(), Role.Admin.ToString() };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    // Create the roles and seed in database
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
