using EShopOnAbp.CatalogService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using System;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService.DbMigrations
{
    public class CatalogServiceDatabaseMigrationChecker : PendingMigrationsCheckerBase<CatalogServiceDbContext>
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
