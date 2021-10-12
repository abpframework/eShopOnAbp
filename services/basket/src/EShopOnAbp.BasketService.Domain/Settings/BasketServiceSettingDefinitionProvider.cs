using Volo.Abp.Settings;

namespace EShopOnAbp.BasketService.Settings
{
    public class BasketServiceSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BasketServiceSettings.MySetting1));
        }
    }
}
