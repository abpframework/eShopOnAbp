using EShopOnAbp.CmskitService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.CmskitService;

public abstract class CmskitServiceAppService : ApplicationService
{
    protected CmskitServiceAppService()
    {
        LocalizationResource = typeof(CmskitServiceResource);
        ObjectMapperContext = typeof(CmskitServiceApplicationModule);
    }
}
