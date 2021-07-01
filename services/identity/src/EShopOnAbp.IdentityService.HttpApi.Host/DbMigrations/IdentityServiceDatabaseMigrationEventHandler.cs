using EShopOnAbp.IdentityService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class IdentityServiceDatabaseMigrationEventHandler
        : DatabaseMigrationEventHandlerBase<IdentityServiceDbContext>,
            IDistributedEventHandler<TenantCreatedEto>,
            IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;
        private readonly IdentityServerDataSeeder _identityServerDataSeeder;

        public IdentityServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IIdentityDataSeeder identityDataSeeder,
            IdentityServerDataSeeder identityServerDataSeeder,
            ITenantRepository tenantRepository,
            IDistributedEventBus distributedEventBus
        ) : base(
            currentTenant,
            unitOfWorkManager,
            tenantStore,
            tenantRepository,
            distributedEventBus,
            IdentityServiceDbProperties.ConnectionStringName)
        {
            _identityDataSeeder = identityDataSeeder;
            _identityServerDataSeeder = identityServerDataSeeder;
        }

        public async Task HandleEventAsync(ApplyDatabaseMigrationsEto eventData)
        {
            if (eventData.DatabaseName != DatabaseName)
            {
                return;
            }

            try
            {
                var schemaMigrated = await MigrateDatabaseSchemaAsync(eventData.TenantId);
                await SeedDataAsync(
                    tenantId: eventData.TenantId,
                    adminEmail: IdentityServiceDbProperties.DefaultAdminEmailAddress,
                    adminPassword: IdentityServiceDbProperties.DefaultAdminPassword
                );

                if (eventData.TenantId == null && schemaMigrated)
                {
                    /* Migrate tenant databases after host migration */
                    await QueueTenantMigrationsAsync();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
            }
        }

        public async Task HandleEventAsync(TenantCreatedEto eventData)
        {
            try
            {
                await MigrateDatabaseSchemaAsync(eventData.Id);
                await SeedDataAsync(
                    tenantId: eventData.Id,
                    adminEmail: eventData.Properties.GetOrDefault(IdentityDataSeedContributor.AdminEmailPropertyName) ?? IdentityServiceDbProperties.DefaultAdminEmailAddress,
                    adminPassword: eventData.Properties.GetOrDefault(IdentityDataSeedContributor.AdminPasswordPropertyName) ?? IdentityServiceDbProperties.DefaultAdminPassword
                );
            }
            catch (Exception ex)
            {
                await HandleErrorTenantCreatedAsync(eventData, ex);
            }
        }
        private async Task SeedDataAsync(Guid? tenantId, string adminEmail, string adminPassword)
        {
            using (CurrentTenant.Change(tenantId))
            {
                if (tenantId == null)
                {
                    Log.Information($"Seeding IdentityServer data...");
                    await _identityServerDataSeeder.SeedAsync();
                }
                Log.Information($"Seeding user data...");
                await _identityDataSeeder.SeedAsync(
                    adminEmail,
                    adminPassword,
                    tenantId
                );
            }
        }
    }
}
