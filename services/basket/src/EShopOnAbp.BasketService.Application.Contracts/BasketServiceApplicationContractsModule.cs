using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class BasketServiceApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BasketServiceDtoExtensions.Configure();
        }
    }
}
