using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Identity.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        //Entry point for the application
        //The application initially starts as a command line application. 
        //The main method Configures ASP dot net core and starts it.
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var identityContetx = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContetx.Database.MigrateAsync();
                    await AppIdentityDbContextSeed.SeedUSerAsync(userManager);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Anerror occured during migration"); 
                }
            }

            host.Run();
        }

        //CreateHostBuilder calls CreateDefaultBuilder on a static web host class that configure  
        //the web host using defaults. It deals with the configuration on how ASP dotnet core deals 
        //with web server configuration files routing and so on.
        // webBuilder is also configured to use a Startup class file
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
