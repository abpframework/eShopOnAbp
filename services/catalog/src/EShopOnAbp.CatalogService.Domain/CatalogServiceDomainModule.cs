using EShopOnAbp.OrderingService;
using EShopOnAbp.CatalogService.Products;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CatalogService;

[DependsOn(
    typeof(CatalogServiceDomainSharedModule),
    typeof(AbpDddDomainModule),
    typeof(AbpAutoMapperModule)
)]
public class CatalogServiceDomainModule : AbpModule
{
    [DependsOn(
        typeof(CatalogServiceDomainSharedModule),
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(OrderingServiceDomainSharedModule)
    )]
    public class CatalogServiceDomainModule : AbpModule
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CatalogServiceDomainModule>(validate: true); });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<Product>();
            options.EtoMappings.Add<Product, ProductEto>();
        });
    }
}