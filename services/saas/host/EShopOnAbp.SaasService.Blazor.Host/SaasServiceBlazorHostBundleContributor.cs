using Volo.Abp.Bundling;

namespace EShopOnAbp.SaasService.Blazor.Host
{
    public class SaasServiceBlazorHostBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {

        }

        public void AddStyles(BundleContext context)
        {
            context.Add("main.css", true);
        }
    }
}
