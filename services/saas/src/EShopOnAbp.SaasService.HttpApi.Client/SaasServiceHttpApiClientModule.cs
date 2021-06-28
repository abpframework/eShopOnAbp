using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SaasServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "SaasService";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SaasServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
