using System;
using EShopOnAbp.CatalogService.MongoDB;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService.DbMigrations
{
    public class CatalogServiceDatabaseMigrationChecker : PendingMongoDbMigrationsChecker<CatalogServiceMongoDbContext>
    {
        public CatalogServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                CatalogServiceDbProperties.ConnectionStringName)
        {
        }
    }
}
