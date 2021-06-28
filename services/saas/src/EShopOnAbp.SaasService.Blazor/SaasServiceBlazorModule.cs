using Microsoft.Extensions.DependencyInjection;
using EShopOnAbp.SaasService.Blazor.Menus;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace EShopOnAbp.SaasService.Blazor
{
    [DependsOn(
        typeof(SaasServiceApplicationContractsModule),
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAutoMapperModule)
        )]
    public class SaasServiceBlazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SaasServiceBlazorModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SaasServiceBlazorAutoMapperProfile>(validate: true);
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new SaasServiceMenuContributor());
            });

            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(SaasServiceBlazorModule).Assembly);
            });
        }
    }
}