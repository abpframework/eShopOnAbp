using Volo.Abp.Modularity;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceApplicationModule),
    typeof(CmskitServiceDomainTestModule)
    )]
public class CmskitServiceApplicationTestModule : AbpModule
{

}
