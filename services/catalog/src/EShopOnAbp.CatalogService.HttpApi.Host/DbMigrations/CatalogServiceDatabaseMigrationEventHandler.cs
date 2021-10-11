using EShopOnAbp.CatalogService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService.DbMigrations
{
    public class CatalogServiceDatabaseMigrationEventHandler
        : DatabaseMigrationEventHandlerBase<CatalogServiceDbContext>,
        IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        public CatalogServiceDatabaseMigrationEventHandler(ICurrentTenant currentTenant, IUnitOfWorkManager unitOfWorkManager, ITenantStore tenantStore, ITenantRepository tenantRepository, IDistributedEventBus distributedEventBus, string databaseName) : base(currentTenant, unitOfWorkManager, tenantStore, tenantRepository, distributedEventBus, databaseName)
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
