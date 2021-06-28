using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.AspNetCore
{
    [DependsOn(
        typeof(EShopOnAbpSharedHostingModule),
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class EShopOnAbpSharedHostingAspNetCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}