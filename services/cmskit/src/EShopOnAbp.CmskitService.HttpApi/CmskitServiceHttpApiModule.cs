using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using EShopOnAbp.CmskitService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(CmsKitHttpApiModule))]
public class CmskitServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FeatureConfigurer.Configure();
        
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmskitServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CmskitServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
