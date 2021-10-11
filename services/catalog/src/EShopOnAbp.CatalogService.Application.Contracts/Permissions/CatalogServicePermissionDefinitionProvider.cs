using EShopOnAbp.CatalogService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EShopOnAbp.CatalogService.Permissions
{
    public class CatalogServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(CatalogServicePermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(CatalogServicePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CatalogServiceResource>(name);
        }
    }
}
