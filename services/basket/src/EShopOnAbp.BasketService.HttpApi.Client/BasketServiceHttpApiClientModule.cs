using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceApplicationContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class BasketServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Basket";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(BasketServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BasketServiceHttpApiClientModule>();
            });
        }
    }
}