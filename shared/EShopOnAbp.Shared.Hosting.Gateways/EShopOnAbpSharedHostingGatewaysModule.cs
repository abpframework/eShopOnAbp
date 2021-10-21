using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.Gateways
{
    [DependsOn(
        typeof(EShopOnAbpSharedHostingAspNetCoreModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
    )]
    public class EShopOnAbpSharedHostingGatewaysModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddOcelot(configuration)
                .AddPolly();
        }
    }
}