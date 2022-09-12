using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using EShopOnAbp.CmskitService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CmskitServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
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
