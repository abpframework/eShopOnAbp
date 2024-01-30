using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class CachedLocalizationService :  ISingletonDependency
{
    protected ConcurrentDictionary<string, ApplicationLocalizationDto> LocalizationDictionary { get; }

    public CachedLocalizationService()
    {
        LocalizationDictionary = new ConcurrentDictionary<string, ApplicationLocalizationDto>(StringComparer.OrdinalIgnoreCase);
    }

    //TODO:skip url -> Use ServiceName_Culture as cache key
    public void AddOrUpdate(string serviceNameWithCulture, ApplicationLocalizationDto localizationInfo)
    {
        LocalizationDictionary.AddOrUpdate(serviceNameWithCulture, localizationInfo, (key, value) => localizationInfo);
    }

    public ApplicationLocalizationDto Get(string serviceNameWithCulture)
    {
        return LocalizationDictionary.TryGetValue(serviceNameWithCulture, out var result) ? result : null;
    }

    public IDictionary<string, ApplicationLocalizationDto> GetMultipleLocalizationsAsync(string[] serviceNamesWithCulture)
    {
        return LocalizationDictionary
            .Where(kv => serviceNamesWithCulture.Contains(kv.Key))
            .ToImmutableDictionary();
    }
}