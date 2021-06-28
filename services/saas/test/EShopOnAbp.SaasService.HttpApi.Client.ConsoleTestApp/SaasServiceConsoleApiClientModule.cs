using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class SaasServiceConsoleApiClientModule : AbpModule
    {
        
    }
}
