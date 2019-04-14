using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Brewdude.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SQLitePCL;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Brewdude.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
//            Log.Logger = new LoggerConfiguration()
//                .MinimumLevel.Debug()
//                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//                .Enrich.FromLogContext()
//                .WriteTo.File("../../../logs/Brewdude-.log", rollingInterval: RollingInterval.Month)
//                .CreateLogger();
//            Log.Logger = new LoggerConfiguration()
//                .WriteTo.Console()
//                .WriteTo.Seq("http://localhost:5341")
//                .CreateLogger();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            var host = CreateWebHostBuilder(args).Build();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (IsDatabaseRerollEnabled(args, environment))
            {
                // Seed database
                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<BrewdudeDbContext>();

                        // Drop the tables to recreate them with fresh data every server re-roll
                        Console.WriteLine("Initializing database contexts");
                        var timer = new Stopwatch();
                        timer.Start();
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        BrewdudeDbInitializer.Initialize(context);
                        timer.Stop();
                        Console.WriteLine($"Seeding databases, time to initialize {timer.ElapsedMilliseconds} ms");
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
        
        private static bool IsDatabaseRerollEnabled(string[] args, string environment)
        {
            if (!string.IsNullOrWhiteSpace(environment) && environment.Equals("Development"))
            {
                if (args.Any(arg => arg == "reroll"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
