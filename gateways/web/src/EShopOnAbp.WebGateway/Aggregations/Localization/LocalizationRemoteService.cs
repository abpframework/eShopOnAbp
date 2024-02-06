using EShopOnAbp.WebGateway.Aggregations.Base;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class LocalizationRemoteService : AggregateRemoteServiceBase<ApplicationLocalizationDto>,
    ILocalizationRemoteService, ITransientDependency
{
    public LocalizationRemoteService(ILogger<AggregateRemoteServiceBase<ApplicationLocalizationDto>> logger) :
        base(logger)
    {
    }
}