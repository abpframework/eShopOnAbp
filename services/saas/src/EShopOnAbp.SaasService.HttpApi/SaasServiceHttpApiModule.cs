using Localization.Resources.AbpUi;
using EShopOnAbp.SaasService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class SaasServiceHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SaasServiceHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<SaasServiceResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
