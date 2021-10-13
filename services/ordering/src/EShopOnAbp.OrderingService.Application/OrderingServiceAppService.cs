using EShopOnAbp.OrderingService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.OrderingService
{
    public abstract class OrderingServiceAppService : ApplicationService
    {
        protected OrderingServiceAppService()
        {
            LocalizationResource = typeof(OrderingServiceResource);
            ObjectMapperContext = typeof(OrderingServiceApplicationModule);
        }
    }
}
