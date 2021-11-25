using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CatalogService.EntityFrameworkCore
{
    [DependsOn(
        typeof(CatalogServiceDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
        )]
    public class CatalogServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            CatalogServiceEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CatalogServiceDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(b =>
                {
                    b.MigrationsHistoryTable("__CatalogService_Migrations");
                });
            });
        }
    }
}
