using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace EShopOnAbp.Shared.Hosting.AspNetCore;

[DependsOn(
    typeof(EShopOnAbpSharedHostingModule),
    typeof(EShopOnAbpSharedLocalizationModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class EShopOnAbpSharedHostingAspNetCoreModule : AbpModule
{
}