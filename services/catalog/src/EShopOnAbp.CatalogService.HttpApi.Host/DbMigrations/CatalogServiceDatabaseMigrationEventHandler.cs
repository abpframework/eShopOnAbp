using EShopOnAbp.CatalogService.MongoDB;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.MongoDb;
using System;
using System.Threading.Tasks;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
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
            IAbpDistributedLock distributedLockProvider
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
                Log.Information("CatalogService has acquired lock for db migration...");

                await using (var handle = await DistributedLockProvider.TryAcquireAsync(DatabaseName))
                {
                    if (handle != null)
                    {
                        Log.Information("CatalogService is migrating database...");
                        await MigrateDatabaseSchemaAsync();
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
