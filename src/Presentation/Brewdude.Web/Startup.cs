using System.Reflection;
using AutoMapper;
using Brewdude.Application.Beer.GetAllBeers.Queries;
using Brewdude.Application.Beer.Queries.GetAllBeers;
using Brewdude.Persistence;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brewdude.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register query handler assemblies
            var assemblies = new Assembly[]
            {
                typeof(GetAllBeersQueryHandler).Assembly
            };

            services.AddAutoMapper();
            
            services.AddDbContext<BrewdudeDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Brewdude")));

            services.AddMediatR(assemblies);
            
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
