using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public abstract class AggregateServiceBase<TDto>
{
    private readonly IAggregateRemoteService<TDto> _remoteService;

    public AggregateServiceBase(IAggregateRemoteService<TDto> remoteService)
    {
        _remoteService = remoteService;
    }

    public virtual async Task<Dictionary<string, TDto>> GetMultipleFromRemoteAsync(List<string> missingKeys,
        Dictionary<string, string> endpoints)
    {
        return await _remoteService
            .GetMultipleAsync(endpoints
                .Where(kv => missingKeys.Contains(kv.Key))
                .ToDictionary(k => k.Key, v => v.Value));
    }

    public List<string> GetMissingServiceKeys(
        IDictionary<string, TDto> serviceNamesWithData,
        Dictionary<string, string> serviceNamesWithUrls)
    {
        List<string> missingKeysInCache = serviceNamesWithUrls.Keys.Except(serviceNamesWithData.Keys).ToList();
        List<string> missingKeysInUrls = serviceNamesWithData.Keys.Except(serviceNamesWithUrls.Keys).ToList();

        return missingKeysInCache.Concat(missingKeysInUrls).ToList();
    }
}