using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class CachedLocalizationService :  ISingletonDependency
{
    protected static ConcurrentDictionary<string, ApplicationLocalizationDto> LocalizationDictionary { get; private set; }

    public CachedLocalizationService()
    {
        LocalizationDictionary = new ConcurrentDictionary<string, ApplicationLocalizationDto>(StringComparer.OrdinalIgnoreCase);
    }

    public void AddOrUpdate(string serviceNameWithCulture, ApplicationLocalizationDto localizationInfo)
    {
        LocalizationDictionary.AddOrUpdate(serviceNameWithCulture, localizationInfo, (key, value) => localizationInfo);
    }

    public IDictionary<string, ApplicationLocalizationDto> GetLocalizationsFromCacheAsync(string[] serviceNamesWithCulture)
    {
        return LocalizationDictionary
            .Where(kv => serviceNamesWithCulture.Contains(kv.Key))
            .ToDictionary();
    }
}