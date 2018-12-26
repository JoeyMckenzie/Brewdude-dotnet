using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Brewdude.Application.Beer.Commands.CreateBeer;
using Brewdude.Application.Beer.Commands.DeleteBeer;
using Brewdude.Application.Beer.Commands.UpdateBeer;
using Brewdude.Application.Beer.GetAllBeers.Queries;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Application.Beer.Queries.GetBeerById;
using Brewdude.Application.Infrastructure;
using Brewdude.Application.Infrastructure.AutoMapper;
using Brewdude.Application.Security;
using Brewdude.Application.User.Commands.CreateUser;
using Brewdude.Application.User.Queries.GetUserById;
using Brewdude.Application.User.Queries.GetUserByUsername;
using Brewdude.Jwt.Services;
using Brewdude.Persistence;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Brewdude.Web
{
    public class Startup
    {
        private string _jwtSecret;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _jwtSecret = Configuration["Brewdude:JwtSecret"];
            var jwtSigningSecret = Encoding.ASCII.GetBytes(_jwtSecret);
            
            // Register query handler assemblies
            var assemblies = new Assembly[]
            {
                typeof(GetAllBeersQueryHandler).GetTypeInfo().Assembly,
                typeof(GetBeerByIdQueryHandler).GetTypeInfo().Assembly,
                typeof(CreateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(DeleteBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(UpdateBeerCommandHandler).GetTypeInfo().Assembly,
                typeof(CreateUserCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByIdCommandHandler).GetTypeInfo().Assembly,
                typeof(GetUserByUsernameCommandHandler).GetTypeInfo().Assembly
            };

            // Add services
            services.AddAutoMapper(typeof(MappingProfile).GetTypeInfo().Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<ITokenService>(_ => new TokenService(_jwtSecret));
            
            // Add EF Core
            services.AddDbContext<BrewdudeDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Brewdude")));

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddMediatR(assemblies);
            
            // Add JWT options
            // JWT authentication
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
                            var userId = int.Parse(context.Principal.Identity.Name);
//                            var user = userService.GetUserById(userId);
//                            if (user == null)
//                            {
//                                // return unauthorized if user no longer exists
//                                Log.Error("Startup::ConfigureServices - User with userId [{0}] no longer exists", userId);
//                                context.Fail("Unauthorized");
//                            }
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
                        ValidateAudience = false,
                    };
                });


            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<CreateBeerCommandValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<UpdateBeerCommandValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<GetUserByUsernameCommandValidator>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
