using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // So what we need to do is get our service, our data context service, so that we can pass it to our seed method.
            // And what we do for this, we say using var scope equals and then host.services and CreateScope.
            // what we're doing here is creating a scope for the services that we're going to create in this part
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            // And when we're in the main methods inside the program class, we're outside of our middleware.
            // So even though we spent a bunch of time setting up a global exception handler, we don't have access to it in this method.
            // So what we do need to do here is write a try catch block so that we can catch any exceptions that happen 
            try
            {
                var context = services.GetRequiredService<DataContext>();
                // This asynchronously applies any pending migrations for the context to the database, and it will create the database if it does not already exist.
                // So what we're doing here, and this is for our convenience, is what we've been doing so far is using dotnet ef database update.
                // What we're going to do in future is just restart our application to apply any migrations.
                // And what this also means if we drop our database, then all we need to do is restart our application and our database will be recreated.
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context);
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occurred during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
