using EShopOnAbp.PaymentService.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Medallion.Threading;
using Microsoft.Extensions.Logging;

namespace EShopOnAbp.PaymentService.DbMigrations
{
    public class PaymentServiceDatabaseMigrationEventHandler
        : DatabaseEfCoreMigrationEventHandler<PaymentServiceDbContext>,
        IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        public PaymentServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IDistributedEventBus distributedEventBus,
            IDistributedLockProvider distributedLockProvider)
            : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                PaymentServiceDbProperties.ConnectionStringName,
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
                Logger.LogInformation("PaymentService - Before Acquire ");

                await using (var handle = await DistributedLockProvider.AcquireLockAsync(DatabaseName))
                {
                    if (handle != null)
                    {
                        await MigrateDatabaseSchemaAsync(null);
                        Logger.LogInformation("PaymentService No Seed...");

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
