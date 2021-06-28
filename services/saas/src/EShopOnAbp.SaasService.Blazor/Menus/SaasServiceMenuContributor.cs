using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace EShopOnAbp.SaasService.Blazor.Menus
{
    public class SaasServiceMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            //Add main menu items.
            context.Menu.AddItem(new ApplicationMenuItem(SaasServiceMenus.Prefix, displayName: "SaasService", "/SaasService", icon: "fa fa-globe"));
            
            return Task.CompletedTask;
        }
    }
}