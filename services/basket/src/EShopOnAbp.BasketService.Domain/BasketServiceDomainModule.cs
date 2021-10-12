using EShopOnAbp.BasketService.MultiTenancy;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

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
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }
    }
}
