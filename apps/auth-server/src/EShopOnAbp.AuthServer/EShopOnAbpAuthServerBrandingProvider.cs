using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EShopOnAbp.AuthServer
{
    [Dependency(ReplaceServices = true)]
    public class EShopOnAbpAuthServerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "EShopOnAbp";
    }
}
