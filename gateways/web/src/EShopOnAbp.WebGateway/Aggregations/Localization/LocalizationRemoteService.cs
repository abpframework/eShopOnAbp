using EShopOnAbp.WebGateway.Aggregations.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class LocalizationRemoteService : AggregateRemoteServiceBase<ApplicationLocalizationDto>,
    ILocalizationRemoteService, ITransientDependency
{
    public LocalizationRemoteService(
        IHttpContextAccessor httpContextAccessor,
        IJsonSerializer jsonSerializer,
        ILogger<AggregateRemoteServiceBase<ApplicationLocalizationDto>> logger)
        : base(httpContextAccessor, jsonSerializer, logger)
    {
    }
}