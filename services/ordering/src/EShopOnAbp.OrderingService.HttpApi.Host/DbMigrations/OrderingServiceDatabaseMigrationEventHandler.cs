using EShopOnAbp.OrderingService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.OrderingService.DbMigrations
{
    public class OrderingServiceDatabaseMigrationEventHandler
        : DatabaseEfCoreMigrationEventHandler<OrderingServiceDbContext>,
        IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        private readonly IDataSeeder _dataSeeder;

        public OrderingServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IDistributedEventBus distributedEventBus,
            IDataSeeder dataSeeder,
            IDistributedLockProvider distributedLockProvider)
            : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                OrderingServiceDbProperties.ConnectionStringName,
                distributedLockProvider)
        {
            _dataSeeder = dataSeeder;
        }

        public async Task HandleEventAsync(ApplyDatabaseMigrationsEto eventData)
        {
            Logger.LogInformation("OrderingService - HandleEventAsync started ...");

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
                Logger.LogInformation("OrderingService - Before Acquire ");

                await using (var handle = await DistributedLockProvider.AcquireLockAsync(DatabaseName))
                {
                    if (handle != null)
                    {
                        await MigrateDatabaseSchemaAsync(null);
                        Logger.LogInformation("Starting OrderingService DataSeeder...");
                        await _dataSeeder.SeedAsync();
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
