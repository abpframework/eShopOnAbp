using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using System;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.MongoDB;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.MongoDb;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService.DbMigrations
{
    public class CatalogServiceDatabaseMigrationEventHandler
        : DatabaseMongoDbMigrationEventHandler<CatalogServiceMongoDbContext>,
        IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        public CatalogServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IDistributedEventBus distributedEventBus,
            IServiceProvider serviceProvider
            ) : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                CatalogServiceDbProperties.ConnectionStringName,serviceProvider)
        {
        }

        public async Task HandleEventAsync(ApplyDatabaseMigrationsEto eventData)
        {
            if (eventData.DatabaseName != DatabaseName)
            {
                return;
            }

            if (eventData.TenantId != null)
            {
                return;
            }

            try
            {
                await MigrateDatabaseSchemaAsync(null);
            }
            catch (Exception ex)
            {
                await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
            }
        }
    }
}
