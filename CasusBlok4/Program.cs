using CasusBlok4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
#if DEBUG
            using IServiceScope scope = host.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");
            try
            {
                DataContext dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (dataContext.Database.EnsureCreated())
                {
                    logger.LogWarning("The database was empty, ef core automatically created the tables. For a development environment it is fine, but in production this will lead to an exception");
                    
                    logger.LogDebug("Starting seeder...");

                    string productSeedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/Mock/Products.sql");
                    dataContext.Database.ExecuteSqlRaw(File.ReadAllText(productSeedFilePath));

                    string customerSeedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/Mock/Customers.sql");
                    dataContext.Database.ExecuteSqlRaw(File.ReadAllText(customerSeedFilePath));

                    dataContext.SaveChanges();

                    logger.LogDebug("Seeding has succeeded");
                }
            }
            catch (Exception e)
            {
                logger.LogDebug(e, "Seeding has failed");
            }
#endif

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
