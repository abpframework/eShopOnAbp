using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.WebGateway.Aggregations.Localization;

public class RemoteLocalizationService : IRemoteLocalizationService, ITransientDependency
{
    private readonly ILogger<RemoteLocalizationService> _logger;

    public RemoteLocalizationService(ILogger<RemoteLocalizationService> logger)
    {
        _logger = logger;
    }
    
    public async Task<Dictionary<string, ApplicationLocalizationDto>> GetMultipleLocalizationsAsync(Dictionary<string, string> serviceNameWithUrlDictionary)
    {
        Dictionary<string, Task<ApplicationLocalizationDto>> completedTasks = new Dictionary<string, Task<ApplicationLocalizationDto>>();
        Dictionary<string, Task<ApplicationLocalizationDto>> runningTasks = new Dictionary<string, Task<ApplicationLocalizationDto>>();
        Dictionary<string, ApplicationLocalizationDto> completedResult = new Dictionary<string, ApplicationLocalizationDto>();

        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var service in serviceNameWithUrlDictionary)
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

                    _logger.LogInformation($"Localization Key: {completedTask.Key}, Value: {result}");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error for the {completedTask.Key}: {ex.Message}");
                }
            }
        }

        return completedResult;
    }

    private async Task<KeyValuePair<string, Task<ApplicationLocalizationDto>>> WaitForAnyTaskAsync(Dictionary<string, Task<ApplicationLocalizationDto>> tasks)
    {
        Task<Task<ApplicationLocalizationDto>> completedTask = Task.WhenAny(tasks.Values);
        Task<ApplicationLocalizationDto> result = await completedTask;

        KeyValuePair<string, Task<ApplicationLocalizationDto>> completedTaskPair = tasks.First(kv => kv.Value == result);

        return completedTaskPair;
    }

    private async Task<T> MakeRequestAsync<T>(HttpClient httpClient, string url)
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
}