using EShopOnAbp.PaymentService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
using System.Threading.Tasks;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.PaymentService.DbMigrations;

public class PaymentServiceDatabaseMigrationEventHandler
    : DatabaseEfCoreMigrationEventHandler<PaymentServiceDbContext>,
        IDistributedEventHandler<ApplyDatabaseMigrationsEto>
{
    public PaymentServiceDatabaseMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock distributedLockProvider)
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
            Log.Information("PaymentService has acquired lock for db migration...");

            await using (var handle = await DistributedLockProvider.TryAcquireAsync(DatabaseName))
            {
                if (handle != null)
                {
                    Log.Information("PaymentService is migrating database...");
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