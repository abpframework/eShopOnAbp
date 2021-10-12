using System;
using System.Collections.Generic;
using System.Text;
using EShopOnAbp.BasketService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.BasketService
{
    /* Inherit your application services from this class.
     */
    public abstract class BasketServiceAppService : ApplicationService
    {
        protected BasketServiceAppService()
        {
            LocalizationResource = typeof(BasketServiceResource);
        }
    }
}
