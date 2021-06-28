using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(SaasServiceDomainSharedModule)
    )]
    public class SaasServiceDomainModule : AbpModule
    {

    }
}
