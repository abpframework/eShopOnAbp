using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EShopOnAbp.CatalogService.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class CatalogServiceDbContextFactory : IDesignTimeDbContextFactory<CatalogServiceDbContext>
    {
        public CatalogServiceDbContext CreateDbContext(string[] args)
        {
            CatalogServiceEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new CatalogServiceDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EShopOnAbp.CatalogService.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
