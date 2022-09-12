using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CmskitServiceApplicationContractsModule : AbpModule
{

}
