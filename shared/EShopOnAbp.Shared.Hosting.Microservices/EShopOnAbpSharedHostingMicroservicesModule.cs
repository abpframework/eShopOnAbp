using System;
using EShopOnAbp.SaasService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace EShopOnAbp.Shared.Hosting.Microservices
{
    [DependsOn(
        typeof(EShopOnAbpSharedHostingAspNetCoreModule),
        typeof(AbpBackgroundJobsRabbitMqModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(SaasServiceEntityFrameworkCoreModule)
        // typeof(AdministrationServiceEntityFrameworkCoreModule)
    )]
    public class EShopOnAbpSharedHostingMicroservicesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = true;
            });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "EShopOnAbp:";
            });

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "EShopOnAbp-Protection-Keys");
        }
    }
}