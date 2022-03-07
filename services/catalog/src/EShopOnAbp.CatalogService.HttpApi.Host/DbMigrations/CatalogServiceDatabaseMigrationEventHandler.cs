using System;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.MongoDB;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.MongoDb;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
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
            IServiceProvider serviceProvider,
            IDistributedLockProvider distributedLockProvider
            ) : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                CatalogServiceDbProperties.ConnectionStringName,
                serviceProvider,
                distributedLockProvider)
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
                Logger.LogInformation("CatalogService - Before Acquire ");

                await using (var handle = await DistributedLockProvider.AcquireLockAsync(DatabaseName))
                {
                    if (handle != null)
                    {
                        await MigrateDatabaseSchemaAsync(null);
                    }
                }
            }
            catch (Exception ex)
            {
                await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
            }
        }
    }
}
