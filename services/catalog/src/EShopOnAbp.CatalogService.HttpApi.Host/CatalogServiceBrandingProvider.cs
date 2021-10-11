using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EShopOnAbp.CatalogService
{
    [Dependency(ReplaceServices = true)]
    public class CatalogServiceBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "CatalogService";
    }
}
