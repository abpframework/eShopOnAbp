using EShopOnAbp.WebGateway.Aggregations.Base;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public class AppConfigurationRemoteService(ILogger<AggregateRemoteServiceBase<ApplicationConfigurationDto>> logger, IJsonSerializer jsonSerializer)
    : AggregateRemoteServiceBase<ApplicationConfigurationDto>(logger, jsonSerializer),
        IAppConfigurationRemoteService, ITransientDependency;