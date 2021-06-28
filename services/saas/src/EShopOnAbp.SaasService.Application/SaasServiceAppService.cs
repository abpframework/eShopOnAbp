using EShopOnAbp.SaasService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.SaasService
{
    public abstract class SaasServiceAppService : ApplicationService
    {
        protected SaasServiceAppService()
        {
            LocalizationResource = typeof(SaasServiceResource);
            ObjectMapperContext = typeof(SaasServiceApplicationModule);
        }
    }
}
