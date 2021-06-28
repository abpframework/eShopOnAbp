using System.Threading.Tasks;
using EShopOnAbp.SaasService.Localization;
using Volo.Abp.UI.Navigation;

namespace EShopOnAbp.SaasService.Blazor.Host
{
    public class SaasServiceHostMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if(context.Menu.DisplayName != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var l = context.GetLocalizer<SaasServiceResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    "SaasService.Home",
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home"
                )
            );

            return Task.CompletedTask;
        }
    }
}
