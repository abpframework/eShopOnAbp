using EShopOnAbp.SaasService.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.SaasService.Pages
{
    public abstract class SaasServicePageModel : AbpPageModel
    {
        protected SaasServicePageModel()
        {
            LocalizationResourceType = typeof(SaasServiceResource);
        }
    }
}