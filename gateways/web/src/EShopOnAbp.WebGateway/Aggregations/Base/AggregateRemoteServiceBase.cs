using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Json;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public abstract class AggregateRemoteServiceBase<TDto> : IAggregateRemoteService<TDto>
{
    private readonly ILogger<AggregateRemoteServiceBase<TDto>> _logger;
    protected IJsonSerializer JsonSerializer { get; }

    protected AggregateRemoteServiceBase(ILogger<AggregateRemoteServiceBase<TDto>> logger, IJsonSerializer jsonSerializer)
    {
        _logger = logger;
        JsonSerializer = jsonSerializer;
    }

    public async Task<Dictionary<string, TDto>> GetMultipleAsync(
        Dictionary<string, string> serviceNameWithUrlDictionary)
    {
        Dictionary<string, Task<TDto>> completedTasks = new Dictionary<string, Task<TDto>>();
        Dictionary<string, Task<TDto>> runningTasks = new Dictionary<string, Task<TDto>>();
        Dictionary<string, TDto> completedResult = new Dictionary<string, TDto>();

        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var service in serviceNameWithUrlDictionary)
            {
                Task<TDto> requestTask =
                    MakeRequestAsync<TDto>(httpClient, service.Value);
                runningTasks.Add(service.Key, requestTask);
            }

            while (runningTasks.Count > 0)
            {
                KeyValuePair<string, Task<TDto>> completedTask = await WaitForAnyTaskAsync(runningTasks);

                runningTasks.Remove(completedTask.Key);

                try
                {
                    TDto result = await completedTask.Value;

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

    public async Task<T> MakeRequestAsync<T>(HttpClient httpClient, string url)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
        catch (Exception e)
        {
            _logger.LogInformation($"Error making request to {url}: {e.Message}");
            throw;
        }
    }

    public async Task<KeyValuePair<TKey, Task<TValue>>> WaitForAnyTaskAsync<TKey, TValue>(
        Dictionary<TKey, Task<TValue>> tasks)
    {
        var completedTask = Task.WhenAny(tasks.Values);
        var result = await completedTask;

        var completedTaskPair = tasks.First(kv => kv.Value == result);

        return completedTaskPair;
    }
}