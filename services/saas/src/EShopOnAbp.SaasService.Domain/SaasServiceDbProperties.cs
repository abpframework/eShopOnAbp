namespace EShopOnAbp.SaasService
{
    public static class SaasServiceDbProperties
    {
        public static string DbTablePrefix { get; set; } = "SaasService";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "SaasService";
    }
}
