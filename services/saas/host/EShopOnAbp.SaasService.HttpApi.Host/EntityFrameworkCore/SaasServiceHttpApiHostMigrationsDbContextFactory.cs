using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    public class SaasServiceHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<SaasServiceHttpApiHostMigrationsDbContext>
    {
        public SaasServiceHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<SaasServiceHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("SaasService"));

            return new SaasServiceHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
