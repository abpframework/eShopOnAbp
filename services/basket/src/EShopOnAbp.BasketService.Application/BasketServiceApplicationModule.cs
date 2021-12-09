using EShopOnAbp.CatalogService;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceDomainModule),
        typeof(BasketServiceApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(CatalogServiceHttpApiClientModule)
        )]
    public class BasketServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BasketServiceApplicationModule>();
            });
        }
    }
}
