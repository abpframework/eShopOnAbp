using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.SaasService
{
    [Dependency(ReplaceServices = true)]
    public class SaasServiceBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "SaasService";
    }
}
