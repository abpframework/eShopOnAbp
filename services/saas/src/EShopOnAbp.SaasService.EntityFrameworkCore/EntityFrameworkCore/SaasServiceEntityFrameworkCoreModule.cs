using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    [DependsOn(
        typeof(SaasServiceDomainModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
    )]
    public class SaasServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SaasServiceDbContext>(options =>
            {
                options.ReplaceDbContext<ITenantManagementDbContext>();
                
                /* includeAllEntities: true allows to use IRepository<TEntity, TKey> also for non aggregate root entities */
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            
            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<SaasServiceDbContext>(c =>
                {
                    c.UseSqlServer(b =>
                    {
                        b.MigrationsHistoryTable("__SaasService_Migrations");
                    });
                });
            });
        }
    }
}