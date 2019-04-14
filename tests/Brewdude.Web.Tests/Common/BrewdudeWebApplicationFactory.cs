using System;
using Brewdude.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brewdude.Web.Tests.Common
{
    public class BrewdudeWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> 
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context using an in-memory 
                // database for testing.
                services.AddDbContext<BrewdudeDbContext>(options =>
                {
                    options.UseInMemoryDatabase("BrewdudeWebTestDatabase");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var serviceProviders = services.BuildServiceProvider();
                
                // Create a scope to obtain a reference to the database
                using (var scope = serviceProviders.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var brewdudeContext = scopedServices.GetRequiredService<BrewdudeDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<BrewdudeWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    brewdudeContext.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbContexts(brewdudeContext);
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(exception, $"An error occurred seeding the database with test messages. Error: {exception.Message}");
                    }
                }
            });
        }
    }
}