using EShopOnAbp.CatalogService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CatalogService
{
    [DependsOn(
        typeof(CatalogServiceEntityFrameworkCoreTestModule)
        )]
    public class CatalogServiceDomainTestModule : AbpModule
    {

    }
}