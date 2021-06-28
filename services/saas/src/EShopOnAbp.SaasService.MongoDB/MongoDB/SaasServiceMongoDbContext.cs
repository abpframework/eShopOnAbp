using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.SaasService.MongoDB
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public class SaasServiceMongoDbContext : AbpMongoDbContext, ISaasServiceMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureSaasService();
        }
    }
}