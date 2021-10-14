using EShopOnAbp.OrderingService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EShopOnAbp.OrderingService.Permissions
{
    public class OrderingServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(OrderingServicePermissions.GroupName, L("Permission:OrderingService"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OrderingServiceResource>(name);
        }
    }
}