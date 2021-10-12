using Volo.Abp.Reflection;

namespace EShopOnAbp.OrderingService.Permissions
{
    public class OrderingServicePermissions
    {
        public const string GroupName = "OrderingService";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderingServicePermissions));
        }
    }
}