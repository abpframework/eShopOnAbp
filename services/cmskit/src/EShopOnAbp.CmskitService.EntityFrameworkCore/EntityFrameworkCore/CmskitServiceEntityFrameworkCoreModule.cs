using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.CmsKit.EntityFrameworkCore;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

[DependsOn(
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(CmskitServiceDomainModule),
    typeof(CmsKitEntityFrameworkCoreModule))]

public class CmskitServiceEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CmskitServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CmskitServiceDbContext>(options =>
        {
            options.ReplaceDbContext<ICmskitServiceDbContext>();
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also CmskitServiceMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql(b =>
            {
                b.MigrationsHistoryTable("__CmskitService_Migrations");
            });
        });
    }
}
