using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.WebGateway.Aggregations.Localization;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace EShopOnAbp.WebGateway.Aggregations;

public class LocalizationAggregation : ILocalizationAggregation
{
    //Yarp localization route name
    public string LocalizationRouteName { get; } = "EshopOnAbpLocalization";
    public string LocalizationEndpoint { get; } = "api/abp/application-localization";

    private readonly CachedLocalizationService _cachedLocalizationService;
    private readonly IRemoteLocalizationService _remoteLocalizationService;

    public LocalizationAggregation(CachedLocalizationService cachedLocalizationService,
        IRemoteLocalizationService remoteLocalizationService)
    {
        _cachedLocalizationService = cachedLocalizationService;
        _remoteLocalizationService = remoteLocalizationService;
    }


    public async Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input)
    {
        // Check the cache service
        var cachedLocalization = _cachedLocalizationService
            .GetLocalizationsFromCacheAsync(input.LocalizationEndpoints.Keys.ToArray());

        // Compare cache with input service list
        var missingLocalizationKeys = GetMissingLocalizations(cachedLocalization, input.LocalizationEndpoints);

        if (missingLocalizationKeys.Count != 0)
        {
            // Make request to remote localization service to get missing localizations
            var remoteLocalizationResults =
                await GetLocalizationsFromRemoteAsync(missingLocalizationKeys, input.LocalizationEndpoints);

            // Update localization cache
            foreach (var result in remoteLocalizationResults)
            {
                _cachedLocalizationService.AddOrUpdate(result.Key, result.Value);
            }

            cachedLocalization = _cachedLocalizationService
                .GetLocalizationsFromCacheAsync(input.LocalizationEndpoints.Keys.ToArray());
        }

        //merge result
        ApplicationLocalizationDto mergedResult = MergeLocalizations(cachedLocalization);

        //return result
        return mergedResult;
    }

    private async Task<Dictionary<string, ApplicationLocalizationDto>> GetLocalizationsFromRemoteAsync(
        List<string> missingLocalizationKeys, Dictionary<string, string> localizationEndpoints)
    {
        return await _remoteLocalizationService
            .GetMultipleLocalizationsAsync(localizationEndpoints
                .Where(kv => missingLocalizationKeys.Contains(kv.Key))
                .ToDictionary(k => k.Key, v => v.Value));
    }

    private List<string> GetMissingLocalizations(
        IDictionary<string, ApplicationLocalizationDto> serviceNameWithLocalization,
        Dictionary<string, string> serviceNameWithUrls)
    {
        List<string> missingKeysInCache = serviceNameWithUrls.Keys.Except(serviceNameWithLocalization.Keys).ToList();
        List<string> missingKeysInUrls = serviceNameWithLocalization.Keys.Except(serviceNameWithUrls.Keys).ToList();

        return missingKeysInCache.Concat(missingKeysInUrls).ToList();
    }

    private ApplicationLocalizationDto MergeLocalizations(
        IDictionary<string, ApplicationLocalizationDto> localizationResults)
    {
        var localizationDto = new ApplicationLocalizationDto();

        foreach (var localization in localizationResults)
        {
            foreach (var resource in localization.Value.Resources)
            {
                localizationDto.Resources.TryAdd(resource.Key, resource.Value);
            }
        }

        return localizationDto;
    }
}

public class LocalizationRequest
{
    // Localization clusterName - localization endpoint pair
    public Dictionary<string, string> LocalizationEndpoints { get; }
    public string CultureName { get; set; }

    public LocalizationRequest(string cultureName)
    {
        CultureName = cultureName;
        LocalizationEndpoints = new Dictionary<string, string>();
    }
}