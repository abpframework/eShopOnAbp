using EShopOnAbp.SaasService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EShopOnAbp.SaasService.Permissions
{
    public class SaasServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(SaasServicePermissions.GroupName, L("Permission:SaasService"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SaasServiceResource>(name);
        }
    }
}