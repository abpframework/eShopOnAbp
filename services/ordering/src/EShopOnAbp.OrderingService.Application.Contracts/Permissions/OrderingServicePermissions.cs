using Volo.Abp.Reflection;

namespace EShopOnAbp.OrderingService.Permissions
{
    public static class OrderingServicePermissions
    {
        public const string GroupName = "OrderingService";

        public static class Orders
        {
            public const string Default = GroupName + ".Orders";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderingServicePermissions));
        }
    }
}