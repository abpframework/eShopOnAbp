using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace EShopOnAbp.WebGateway.Aggregations.Base;

public abstract class CachedServiceBase<TCacheValue> : ICachedServiceBase<TCacheValue>
{
    private readonly IMemoryCache _cache;

    protected MemoryCacheEntryOptions CacheEntryOptions { get; } = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
        SlidingExpiration = TimeSpan.FromHours(4)
    };

    protected CachedServiceBase(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public void Add(string serviceName, TCacheValue data)
    {
        _cache.Set(serviceName, data, CacheEntryOptions);
    }

    public IDictionary<string, TCacheValue> GetManyAsync(IEnumerable<string> serviceNames)
    {
        var result = new Dictionary<string, TCacheValue>();

        foreach (var serviceName in serviceNames)
        {
            if (_cache.TryGetValue(serviceName, out TCacheValue data))
            {
                result.Add(serviceName, data);
            }
        }

        return result;
    }
}