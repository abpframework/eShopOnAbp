using System;
using EShopOnAbp.BasketService.Configs;
using EShopOnAbp.BasketService.Grpc;
using EShopOnAbp.CatalogService;
using EShopOnAbp.CatalogService.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
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
            context.Services.AddOptions();
            context.Services.Configure<UrlsConfig>(configuration.GetSection("urls"));
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BasketServiceApplicationModule>();
            });
            
            context.Services.AddGrpcClient<ProductPublic.ProductPublicClient>((services, options) =>
            {
                var catalogApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcCatalog;
                options.Address = new Uri(catalogApi);
            }).AddInterceptor<GrpcExceptionInterceptor>();
        }
    }
}
