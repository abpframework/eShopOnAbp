using EShopOnAbp.IdentityService.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using Volo.Abp.DistributedLocking;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class IdentityServiceDatabaseMigrationEventHandler
        : DatabaseEfCoreMigrationEventHandler<IdentityServiceDbContext>,
            IDistributedEventHandler<TenantCreatedEto>,
            IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;
        private readonly IdentityServerDataSeeder _identityServerDataSeeder;
        private readonly ILocalEventBus _localEventBus;

        public IdentityServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IIdentityDataSeeder identityDataSeeder,
            IdentityServerDataSeeder identityServerDataSeeder,
            IDistributedEventBus distributedEventBus,
            ILocalEventBus localEventBus,
            IAbpDistributedLock distributedLockProvider
        ) : base(
            currentTenant,
            unitOfWorkManager,
            tenantStore,
            distributedEventBus,
            IdentityServiceDbProperties.ConnectionStringName,
            distributedLockProvider)
        {
            _identityDataSeeder = identityDataSeeder;
            _identityServerDataSeeder = identityServerDataSeeder;
            _localEventBus = localEventBus;
        }

        public async Task HandleEventAsync(ApplyDatabaseMigrationsEto eventData)
        {
            if (eventData.DatabaseName != DatabaseName)
            {
                return;
            }

            try
            {
                await using (var handle = await DistributedLockProvider.TryAcquireAsync(DatabaseName))
                {
                    Log.Information("IdentityService has acquired lock for db migration...");
                    
                    if (handle != null)
                    {
                        Log.Information("IdentityService is migrating database...");
                        await MigrateDatabaseSchemaAsync(eventData.TenantId);
                        Log.Information("IdentityService is seeding data...");
                        await SeedDataAsync(
                            tenantId: eventData.TenantId,
                            adminEmail: IdentityServiceDbProperties.DefaultAdminEmailAddress,
                            adminPassword: IdentityServiceDbProperties.DefaultAdminPassword
                        );
                    }
                }

                await _localEventBus.PublishAsync(new ApplyDatabaseSeedsEto());
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
