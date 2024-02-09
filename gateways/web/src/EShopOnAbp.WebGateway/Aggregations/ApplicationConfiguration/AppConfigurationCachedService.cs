using EShopOnAbp.WebGateway.Aggregations.Base;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;

public class AppConfigurationCachedService(IMemoryCache applicationConfigurationCache)
    : CachedServiceBase<ApplicationConfigurationDto>(applicationConfigurationCache), ISingletonDependency;