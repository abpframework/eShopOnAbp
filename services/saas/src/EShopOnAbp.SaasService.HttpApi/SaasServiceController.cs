using EShopOnAbp.SaasService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.SaasService
{
    public abstract class SaasServiceController : AbpController
    {
        protected SaasServiceController()
        {
            LocalizationResource = typeof(SaasServiceResource);
        }
    }
}
