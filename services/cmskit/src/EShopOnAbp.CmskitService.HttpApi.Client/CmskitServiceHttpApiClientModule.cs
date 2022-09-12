using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CmskitServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(CmskitServiceApplicationContractsModule).Assembly,
            CmskitServiceRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmskitServiceHttpApiClientModule>();
        });
    }
}
