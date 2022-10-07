using EShopOnAbp.DbMigrator.Keycloak;
using EShopOnAbp.Shared.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace EShopOnAbp.DbMigrator;

[DependsOn(
    typeof(EShopOnAbpSharedHostingModule)
)]
public class EShopOnAbpDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        context.Services.AddHttpClient(KeycloakService.HttpClientName);
        
        Configure<KeycloakClientOptions>(configuration);
    }
}