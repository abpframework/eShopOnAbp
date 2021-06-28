using EShopOnAbp.SaasService.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.SaasService.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class SaasServicePageModel : AbpPageModel
    {
        protected SaasServicePageModel()
        {
            LocalizationResourceType = typeof(SaasServiceResource);
            ObjectMapperContext = typeof(SaasServiceWebModule);
        }
    }
}