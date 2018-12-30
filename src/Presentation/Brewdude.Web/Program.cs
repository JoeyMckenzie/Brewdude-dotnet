using System;
using System.Data;
using System.IO;
using System.Linq;
using Brewdude.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Brewdude.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("../../../logs/Brewdude-.log", rollingInterval: RollingInterval.Month)
                .CreateLogger();

            var host = CreateWebHostBuilder(args).Build();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (!string.IsNullOrWhiteSpace(environment) && environment.Equals("Development"))
            {
                // Seed database
                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<BrewdudeDbContext>();

                        // Drop the tables to recreate them with fresh data every server re-roll
                        if (BrewdudeDbInitializer.TablesExist(context))
                        {
                            const string dropBeers = "DROP TABLE Beers;";
                            const string dropBreweries = "DROP TABLE Breweries;";
                            const string dropUsers = "DROP TABLE Users;";
                            const string dropMigrations = "DROP TABLE __EFMigrationsHistory;";
                            const string dropUserBeers = "DROP TABLE UserBeers;";
                            const string dropUserBreweries = "DROP TABLE UserBreweries;";
                            context.Database.ExecuteSqlCommand(dropBeers);
                            context.Database.ExecuteSqlCommand(dropBreweries);
                            context.Database.ExecuteSqlCommand(dropUsers);
                            context.Database.ExecuteSqlCommand(dropMigrations);
                            context.Database.ExecuteSqlCommand(dropUserBeers);
                            context.Database.ExecuteSqlCommand(dropUserBreweries);
                        }

                        context.Database.Migrate();
                        BrewdudeDbInitializer.Initialize(context);
                    }
                    catch (Exception e)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(e, "Could not seed database");
                    }
                }
            }

            try
            {
                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly. Reason: " + e.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>();
    }
}
