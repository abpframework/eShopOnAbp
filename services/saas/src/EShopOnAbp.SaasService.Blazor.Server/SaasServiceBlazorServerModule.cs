using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService.Blazor.Server
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsServerThemingModule),
        typeof(SaasServiceBlazorModule)
        )]
    public class SaasServiceBlazorServerModule : AbpModule
    {
        
    }
}