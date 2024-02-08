using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.Json;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public abstract class AggregateRemoteServiceBase<TDto> : IAggregateRemoteService<TDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AggregateRemoteServiceBase<TDto>> _logger;
    protected IJsonSerializer JsonSerializer { get; }

    protected AggregateRemoteServiceBase(IHttpContextAccessor httpContextAccessor, IJsonSerializer jsonSerializer,
        ILogger<AggregateRemoteServiceBase<TDto>> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        JsonSerializer = jsonSerializer;
        _logger = logger;
    }

    public async Task<Dictionary<string, TDto>> GetMultipleAsync(
        Dictionary<string, string> serviceNameWithUrlDictionary)
    {
        Dictionary<string, Task<TDto>> completedTasks = new Dictionary<string, Task<TDto>>();
        Dictionary<string, Task<TDto>> runningTasks = new Dictionary<string, Task<TDto>>();
        Dictionary<string, TDto> completedResult = new Dictionary<string, TDto>();

        using (HttpClient httpClient = CreateHttpClient())
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

                    _logger.LogInformation("Localization Key: {0}, Value: {1}", completedTask.Key, result);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error for the {0}: {1}", completedTask.Key, ex.Message);
                }
            }
        }

        return completedResult;
    }

    private HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();

        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        if (headers != null)
        {
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToArray());
            }
        }

        return httpClient;
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
            _logger.LogInformation("Error making request to {0}: {1}", url, e.Message);
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