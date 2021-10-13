using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CatalogService
{
    [DependsOn(
        typeof(CatalogServiceDomainSharedModule),
        typeof(AbpDddDomainModule)
    )]
    public class CatalogServiceDomainModule : AbpModule
    {

    }
}
