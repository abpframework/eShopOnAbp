using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.Shared.Hosting.AspNetCore;

[DependsOn(
    typeof(EShopOnAbpSharedHostingModule),
    typeof(EShopOnAbpSharedLocalizationModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class EShopOnAbpSharedHostingAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EShopOnAbpSharedHostingAspNetCoreModule>("EShopOnAbp.Shared.Hosting.AspNetCore");
        });
    }
}