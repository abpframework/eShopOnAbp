using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpDataModule)
    )]
    public class EShopOnAbpSharedHostingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureDatabaseConnections();
        }
        private void ConfigureDatabaseConnections()
        {
            Configure<AbpDbConnectionOptions>(options =>
            {
                options.Databases.Configure("SaasService", database =>
                {
                    database.MappedConnections.Add("AbpTenantManagement");
                    database.IsUsedByTenants = false;
                });

                options.Databases.Configure("AdministrationService", database =>
                {
                    database.MappedConnections.Add("AbpAuditLogging");
                    database.MappedConnections.Add("AbpPermissionManagement");
                    database.MappedConnections.Add("AbpSettingManagement");
                    database.MappedConnections.Add("AbpFeatureManagement");
                    database.MappedConnections.Add("AbpBlobStoring");
                });
                //
                // options.Databases.Configure("IdentityService", database =>
                // {
                //     database.MappedConnections.Add("AbpIdentity");
                //     database.MappedConnections.Add("AbpIdentityServer");
                // });
                //
                // options.Databases.Configure("ProductService", database =>
                // {
                //     database.MappedConnections.Add("ProductService");
                // });
            });
        }
    }
}