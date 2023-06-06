using EShopOnAbp.CatalogService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.CatalogService
{
    /* Inherit your application services from this class.
     */
    public abstract class CatalogServiceAppService : ApplicationService
    {
        protected CatalogServiceAppService()
        {
            LocalizationResource = typeof(CatalogServiceResource);
        }
    }
}
