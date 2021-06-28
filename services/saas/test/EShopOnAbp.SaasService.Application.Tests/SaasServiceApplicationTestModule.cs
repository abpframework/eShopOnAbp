using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceApplicationModule),
        typeof(SaasServiceDomainTestModule)
        )]
    public class SaasServiceApplicationTestModule : AbpModule
    {

    }
}
