using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

    /* This class is needed for EF Core console commands
    * (like Add-Migration and Update-Database commands) */
public class CmskitServiceDbContextFactory : IDesignTimeDbContextFactory<CmskitServiceDbContext>
{
    public CmskitServiceDbContext CreateDbContext(string[] args)
    {
        CmskitServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<CmskitServiceDbContext>()
               .UseNpgsql(
               configuration.GetConnectionString(CmskitServiceDbProperties.ConnectionStringName),
               b =>
               {
                   b.MigrationsHistoryTable("__CmsKitService_Migrations");
               });

        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return new CmskitServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EShopOnAbp.CmsKitService.HttpApi.Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
