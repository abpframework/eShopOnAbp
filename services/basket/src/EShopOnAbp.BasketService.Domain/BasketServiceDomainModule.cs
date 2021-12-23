using System;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceDomainSharedModule),
        typeof(AbpDddDomainModule),
        typeof(AbpCachingModule)
    )]
    public class BasketServiceDomainModule : AbpModule 
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.CacheConfigurators.Add(cacheName =>
                {
                    if (cacheName == CacheNameAttribute.GetCacheName(typeof(Basket)))
                    {
                        return new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromDays(7)
                        };
                    }

                    return null;
                });
            });
        }
    }
}
