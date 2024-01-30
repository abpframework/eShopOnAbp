using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;

namespace EShopOnAbp.WebGateway.Aggregations;

public class LocalizationAggregation : ILocalizationAggregation
{
    //Yarp localization route name
    public string LocalizationEndpoint { get; } = "EshopOnAbpLocalization";
    private readonly ILogger<LocalizationAggregation> _logger;
    private readonly IDistributedCache<LocalizationResponse> _localizationResponseCache;

    public LocalizationAggregation(ILogger<LocalizationAggregation> logger, IDistributedCache<LocalizationResponse> localizationResponseCache)
    {
        _logger = logger;
        _localizationResponseCache = localizationResponseCache;
    }

    public async Task<ApplicationLocalizationDto> GetLocalizationAsync(LocalizationRequest input)
    {
        //Temp list
        Dictionary<string, string> services = new Dictionary<string, string>()
        {
            { "Administration", "https://localhost:44353/api/abp/application-localization?CultureName=en" },
            { "Catalog", "https://localhost:44354/api/abp/application-localization?CultureName=en" }
        };
        
        // Check the cache
        
        

        Dictionary<string, ApplicationLocalizationDto> localizationResults = await GetCompletedLocalizationResultsAsync(services);
        ApplicationLocalizationDto mergedResult = MergeLocalizations(localizationResults);

        return mergedResult;
    }

    private ApplicationLocalizationDto MergeLocalizations(Dictionary<string, ApplicationLocalizationDto> localizationResults)
    {
        var localizationDto = new ApplicationLocalizationDto();
        foreach (var localizationResult in localizationResults)
        {
            localizationDto.Resources.AddIfNotContains(localizationResult.Value.Resources);
        }

        return localizationDto;
    }

    private async Task<Dictionary<string, ApplicationLocalizationDto>> GetCompletedLocalizationResultsAsync(Dictionary<string, string> services)
    {
        Dictionary<string, Task<ApplicationLocalizationDto>> completedTasks = new Dictionary<string, Task<ApplicationLocalizationDto>>();
        Dictionary<string, Task<ApplicationLocalizationDto>> runningTasks = new Dictionary<string, Task<ApplicationLocalizationDto>>();
        Dictionary<string, ApplicationLocalizationDto> completedResult = new Dictionary<string, ApplicationLocalizationDto>();

        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var service in services)
            {
                Task<ApplicationLocalizationDto> requestTask = MakeRequestAsync<ApplicationLocalizationDto>(httpClient, service.Value);
                runningTasks.Add(service.Key, requestTask);
            }

            while (runningTasks.Count > 0)
            {
                KeyValuePair<string, Task<ApplicationLocalizationDto>> completedTask = await WaitForAnyTaskAsync(runningTasks);

                runningTasks.Remove(completedTask.Key);

                try
                {
                    ApplicationLocalizationDto result = await completedTask.Value;

                    completedTasks.Add(completedTask.Key, completedTask.Value);
                    completedResult.Add(completedTask.Key, result);

                    _logger.LogInformation($"Key: {completedTask.Key}, Value: {result}");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error for the {completedTask.Key}: {ex.Message}");
                }
            }
        }

        return completedResult;
    }

    private async Task<KeyValuePair<string, Task<ApplicationLocalizationDto>>> WaitForAnyTaskAsync(
        Dictionary<string, Task<ApplicationLocalizationDto>> tasks)
    {
        Task<Task<ApplicationLocalizationDto>> completedTask = Task.WhenAny(tasks.Values);
        Task<ApplicationLocalizationDto> result = await completedTask;

        // Find the key corresponding to the completed task
        KeyValuePair<string, Task<ApplicationLocalizationDto>>
            completedTaskPair = tasks.First(kv => kv.Value == result);

        return completedTaskPair;
    }

    private async Task<T> MakeRequestAsync<T>(HttpClient httpClient, string url)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            return JsonSerializer.Deserialize<T>(content, options);
        }
        catch (Exception ex)
        {
            // Log or handle the exception, and return a default value or throw it again if needed
            Console.WriteLine($"Error making request to {url}: {ex.Message}");
            throw;
        }
    }
}

public class LocalizationRequest
{
    // Localization clusterName - localization endpoint pair
    public Dictionary<string, string> LocalizationEndpoints { get; }
    public string CultureName { get; set; }
    public bool? OnlyDynamics { get; set; }

    public LocalizationRequest(string cultureName)
    {
        CultureName = cultureName;
        LocalizationEndpoints = new Dictionary<string, string>();
    }
}

public class LocalizationResponse
{
    // Localization url - localization resource pair
    public Dictionary<string, ApplicationLocalizationDto> Localization { get; }
    public string CultureName { get; set; }
    public bool? OnlyDynamics { get; set; }
    public string ServiceName { get; set; }
}