using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EShopOnAbp.WebGateway.Aggregations.Localization;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations;

public class LocalizationAggregation : ILocalizationAggregation
{
    //Yarp localization route name
    public string LocalizationEndpoint { get; } = "EshopOnAbpLocalization";
    private readonly ILogger<LocalizationAggregation> _logger;
    private readonly CachedLocalizationService _cachedLocalizationService;
    private readonly RemoteLocalizationService _remoteLocalizationService;

    public LocalizationAggregation(ILogger<LocalizationAggregation> logger, CachedLocalizationService cachedLocalizationService, RemoteLocalizationService remoteLocalizationService)
    {
        _logger = logger;
        _cachedLocalizationService = cachedLocalizationService;
        _remoteLocalizationService = remoteLocalizationService;
    }

    public async Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input)
    {
        //Temp list
        Dictionary<string, string> services = new Dictionary<string, string>()
        {
            { "Administration_tr", "https://localhost:44353/api/abp/application-localization?cultureName=tr&onlyDynamics=false" },
            { "Catalog_tr", "https://localhost:44354/api/abp/application-localization?cultureName=tr&onlyDynamics=false" }
        };
        
        // Check the cache
        var cachedLocalization = _cachedLocalizationService.GetMultipleLocalizationsAsync(services.Keys.ToArray());
        
        // Compare cache with input service list
        
        // Make request to remote localization service
        var remoteLocalizationResults = await _remoteLocalizationService.GetMultipleLocalizationsAsync(services);
        
        //merge result
        ApplicationLocalizationDto mergedResult = MergeLocalizations(remoteLocalizationResults);
        
        //return result
       
        return mergedResult;
    }

    private ApplicationLocalizationDto MergeLocalizations(Dictionary<string, ApplicationLocalizationDto> localizationResults)
    {
        var localizationDto = new ApplicationLocalizationDto();
        //TODO: fix
        foreach (var localizationResult in localizationResults)
        {
            localizationDto.Resources.AddIfNotContains(localizationResult.Value.Resources);
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