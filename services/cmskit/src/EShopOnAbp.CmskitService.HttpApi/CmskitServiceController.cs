using EShopOnAbp.CmskitService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.CmskitService;

public abstract class CmskitServiceController : AbpControllerBase
{
    protected CmskitServiceController()
    {
        LocalizationResource = typeof(CmskitServiceResource);
    }
}
