using EShopOnAbp.BasketService.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace EShopOnAbp.BasketService;

public class BasketServiceContractsModule : AbpModule 
{
    public const string RemoteServiceName = "Catalog";
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(BasketServiceContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<BasketServiceResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/BasketService");

            options.DefaultResourceType = typeof(BasketServiceResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("BasketService", typeof(BasketServiceResource));
        });
    }
}