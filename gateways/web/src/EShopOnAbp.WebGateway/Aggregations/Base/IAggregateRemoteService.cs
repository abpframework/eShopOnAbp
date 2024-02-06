using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public interface IAggregateRemoteService<TDto>
{
    Task<Dictionary<string, TDto>> GetMultipleAsync(Dictionary<string, string> serviceNameWithUrlDictionary);
    Task<T> MakeRequestAsync<T>(HttpClient httpClient, string url);
    Task<KeyValuePair<TKey, Task<TValue>>> WaitForAnyTaskAsync<TKey, TValue>(Dictionary<TKey, Task<TValue>> tasks);
}