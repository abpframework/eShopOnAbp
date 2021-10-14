using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceDomainSharedModule),
        typeof(AbpDddDomainModule)
    )]
    public class BasketServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
