using System;
using System.Collections.Generic;
using EShopOnAbp.CatalogService;
using EShopOnAbp.CatalogService.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceDomainModule),
        typeof(BasketServiceApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(CatalogServiceHttpApiClientModule)
        )]
    public class BasketServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BasketServiceApplicationModule>();
            });

            context.Services.AddGrpcClient<ProductPublic.ProductPublicClient>((services, options) =>
            {
                var remoteServiceConfigurationProvider = services.GetRequiredService<IRemoteServiceConfigurationProvider>();
                var catalogServiceConfiguration = remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Catalog").Result;
                var catalogGrpcUrl = catalogServiceConfiguration.GetOrDefault("GrpcUrl");
                
                options.Address = new Uri(catalogGrpcUrl);
            });
        }
    }
}
