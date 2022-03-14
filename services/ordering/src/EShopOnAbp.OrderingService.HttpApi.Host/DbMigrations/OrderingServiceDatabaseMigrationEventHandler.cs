using EShopOnAbp.OrderingService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
using System.Threading.Tasks;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.OrderingService.DbMigrations;

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
        IAbpDistributedLock distributedLockProvider)
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
            await using (var handle = await DistributedLockProvider.TryAcquireAsync(DatabaseName))
            {
                Log.Information("OrderingService has acquired lock for db migration...");

                if (handle != null)
                {
                    Log.Information("OrderingService is migrating database...");
                    await MigrateDatabaseSchemaAsync();
                    Log.Information("OrderingService is seeding data...");
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