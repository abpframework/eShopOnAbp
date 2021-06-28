using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.SaasService.MongoDB
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public interface ISaasServiceMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
