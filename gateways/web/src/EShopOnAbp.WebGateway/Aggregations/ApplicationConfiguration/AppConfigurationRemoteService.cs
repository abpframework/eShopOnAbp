using EShopOnAbp.WebGateway.Aggregations.Base;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public class AppConfigurationRemoteService(ILogger<AggregateRemoteServiceBase<ApplicationConfigurationDto>> logger)
    : AggregateRemoteServiceBase<ApplicationConfigurationDto>(logger),
        IAppConfigurationRemoteService, ITransientDependency;