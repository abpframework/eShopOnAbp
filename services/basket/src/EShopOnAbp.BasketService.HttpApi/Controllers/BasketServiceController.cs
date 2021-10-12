using EShopOnAbp.BasketService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.BasketService.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BasketServiceController : AbpControllerBase
    {
        protected BasketServiceController()
        {
            LocalizationResource = typeof(BasketServiceResource);
        }
    }
}