using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.WebGateway.Aggregations.Base;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class LocalizationAggregation : AggregateServiceBase<ApplicationLocalizationDto>, ILocalizationAggregation,
    ITransientDependency
{
    public string LocalizationRouteName => "EshopOnAbpLocalization";
    public string LocalizationEndpoint => "api/abp/application-localization";
    protected LocalizationCachedService LocalizationCachedService { get; }

    public LocalizationAggregation(
        LocalizationCachedService localizationCachedService,
        ILocalizationRemoteService localizationRemoteService) 
        : base(localizationRemoteService)
    {
        LocalizationCachedService = localizationCachedService;
    }

    public async Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input)
    {
        // Check the cache service
        var cachedLocalization = LocalizationCachedService
            .GetManyAsync(input.Endpoints.Keys.ToArray());

        // Compare cache with input service list
        var missingLocalizationKeys = GetMissingServiceKeys(cachedLocalization, input.Endpoints);

        if (missingLocalizationKeys.Count != 0)
        {
            // Make request to remote localization service to get missing localizations
            var remoteLocalizationResults =
                await GetMultipleFromRemoteAsync(missingLocalizationKeys, input.Endpoints);

            // Update localization cache
            foreach (var result in remoteLocalizationResults)
            {
                LocalizationCachedService.Add(result.Key, result.Value);
            }

            cachedLocalization = LocalizationCachedService
                .GetManyAsync(input.Endpoints.Keys.ToArray());
        }

        //merge result
        var mergedResult = MergeLocalizationData(cachedLocalization);

        //return result
        return mergedResult;
    }

    private static ApplicationLocalizationDto MergeLocalizationData(
        IDictionary<string, ApplicationLocalizationDto> resourceDictionary)
    {
        var localizationDto = new ApplicationLocalizationDto();

        foreach (var localization in resourceDictionary)
        {
            foreach (var resource in localization.Value.Resources)
            {
                localizationDto.Resources.TryAdd(resource.Key, resource.Value);
            }
        }

        return localizationDto;
    }
}