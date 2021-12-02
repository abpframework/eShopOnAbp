using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.CatalogService.MongoDB
{
    [ConnectionStringName(CatalogServiceDbProperties.ConnectionStringName)]
    public class CatalogServiceMongoDbContext : AbpMongoDbContext, ICatalogServiceMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureCatalogService();
        }
    }
}