using System;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.MongoDB;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService.DbMigrations
{
    public class CatalogServiceDatabaseMigrationChecker : PendingMongoDbMigrationsChecker<CatalogServiceMongoDbContext>
    {
        private readonly ProductServiceDataSeeder _productServiceDataSeeder;

        public CatalogServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus, 
            ProductServiceDataSeeder productServiceDataSeeder)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                CatalogServiceDbProperties.ConnectionStringName)
        {
            _productServiceDataSeeder = productServiceDataSeeder;
        }

        public override async Task CheckAsync()
        {
            await base.CheckAsync();
            await _productServiceDataSeeder.SeedAsync();
        }
    }
}
