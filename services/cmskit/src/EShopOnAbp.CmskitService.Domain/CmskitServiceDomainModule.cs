using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CmskitServiceDomainSharedModule)
)]
public class CmskitServiceDomainModule : AbpModule
{
}
