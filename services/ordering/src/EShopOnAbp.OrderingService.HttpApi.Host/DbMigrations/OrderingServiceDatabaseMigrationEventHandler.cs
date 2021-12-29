using EShopOnAbp.OrderingService.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Microsoft.Extensions.Logging;
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
            IDataSeeder dataSeeder) 
            : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                OrderingServiceDbProperties.ConnectionStringName)
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
                await MigrateDatabaseSchemaAsync(null);
                Logger.LogInformation("Starting OrderingService DataSeeder...");
                await _dataSeeder.SeedAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
            }
        }
    }
}
