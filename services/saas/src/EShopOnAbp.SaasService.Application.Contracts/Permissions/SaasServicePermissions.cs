using Volo.Abp.Reflection;

namespace EShopOnAbp.SaasService.Permissions
{
    public class SaasServicePermissions
    {
        public const string GroupName = "SaasService";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SaasServicePermissions));
        }
    }
}