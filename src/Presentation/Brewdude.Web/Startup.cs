namespace Brewdude.Web
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Application;
    using Application.Security;
    using Application.User.Commands.CreateUser;
    using AutoMapper;
    using Brewdude.Application.Infrastructure;
    using Brewdude.Application.Infrastructure.AutoMapper;
    using Common.Constants;
    using Common.Utilities;
    using Domain.Entities;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using Infrastructure;
    using Jwt;
    using Jwt.Services;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Middleware.Infrastructure;
    using Newtonsoft.Json;
    using Persistence;
    using Serilog;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private string _jwtSecret;

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

            // Add Identity
            services.AddDefaultIdentity<BrewdudeUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BrewdudeDbContext>()
                .AddDefaultTokenProviders();

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLogger<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddMediatR(BrewdudeRequestHandlers.GetRequestHandlerAssemblies());

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
                        var userContext = context.HttpContext.RequestServices.GetRequiredService<BrewdudeDbContext>();
                        var userId = context.Principal?.Identity?.Name;

                        // No userId found on token
                        if (userId == null)
                        {
                            return Task.CompletedTask;
                        }

                        var user = userContext.Users.Find(userId);
                        if (user == null)
                        {
                            // Return unauthorized if user no longer exists
                            _logger.LogError($"User with userId [{userId}] no longer exists");
                            context.Fail("Unauthorized");
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
                options.AddPolicy("WriteBreweryPolicy", policyBuilder =>
                    policyBuilder.RequireClaim("scopes", new BrewdudeScopes().WriteBrewery));

                options.AddPolicy("WriteBeerPolicy", policyBuilder =>
                    policyBuilder.RequireClaim("scopes", new BrewdudeScopes().WriteBeer));

                options.AddPolicy("BrewdudeUserPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("User", "Admin");
                    policyBuilder.RequireClaim("username");
                    policyBuilder.RequireClaim("scopes", new BrewdudeScopes().GetUserScopes());
                });
                options.AddPolicy("BrewdudeAdminPolicy", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Admin");
                    policyBuilder.RequireClaim("username");
                    policyBuilder.RequireClaim("scopes", new BrewdudeScopes().GetAdminUserScopes());
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

            // Add Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Brewdude API",
                    Version = "v1",
                    Description = "API endpoints for the Brewdude application.",
                    Contact = new Contact
                    {
                        Name = "Joey Mckenzie",
                        Email = "joey.mckenzie27@gmail.com",
                        Url = "https://azurewebsites.joeymckenzie.com"
                    }
                });
            });

            // Override built in model state validation
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            // Add health checks
            services.AddHealthChecks()
                .AddSqlServer(Configuration["Brewdude:ConnectionString"]);
            services.AddHealthChecks()
                .AddDbContextCheck<BrewdudeDbContext>("BrewdudeDbContextHealthCheck");
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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

            // Seed roles in the database
            CreateRoles(serviceProvider).Wait();

            // Configure CORS policy
            app.UseCors(o => o
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // Configure health checks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
            app.UseHealthChecksUI();

            // Configure the error handling pipeline and Identity authentication
            app.UseErrorHandlingMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    $"Brewdude API version {BrewdudeConstants.Version}"));
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
            // Initializing custom roles
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
