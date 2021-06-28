using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class SaasServiceApplicationContractsModule : AbpModule
    {

    }
}
