using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService.Blazor.WebAssembly
{
    [DependsOn(
        typeof(SaasServiceBlazorModule),
        typeof(SaasServiceHttpApiClientModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
        )]
    public class SaasServiceBlazorWebAssemblyModule : AbpModule
    {
        
    }
}