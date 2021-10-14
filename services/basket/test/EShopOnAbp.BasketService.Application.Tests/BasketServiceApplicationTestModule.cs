using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceApplicationModule),
        typeof(BasketServiceDomainTestModule)
        )]
    public class BasketServiceApplicationTestModule : AbpModule
    {

    }
}