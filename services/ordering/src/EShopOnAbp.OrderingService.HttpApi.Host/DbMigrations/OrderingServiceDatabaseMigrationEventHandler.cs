using EShopOnAbp.OrderingService.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
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
        public OrderingServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IDistributedEventBus distributedEventBus) 
            : base(
                currentTenant,
                unitOfWorkManager,
                tenantStore,
                distributedEventBus,
                OrderingServiceDbProperties.ConnectionStringName)
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
