using EShopOnAbp.AdministrationService.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.AdministrationService.DbMigrations
{
    public class AdministrationServiceDatabaseMigrationEventHandler
    : DatabaseEfCoreMigrationEventHandler<AdministrationServiceDbContext>,
            IDistributedEventHandler<TenantCreatedEto>,
            IDistributedEventHandler<ApplyDatabaseMigrationsEto>
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public AdministrationServiceDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionDataSeeder permissionDataSeeder,
            IDistributedEventBus distributedEventBus
        ) : base(
            currentTenant,
            unitOfWorkManager,
            tenantStore,
            distributedEventBus,
            AdministrationServiceDbProperties.ConnectionStringName)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _permissionDataSeeder = permissionDataSeeder;
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
                await SeedDataAsync(eventData.TenantId);
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
                await SeedDataAsync(eventData.Id);
            }
            catch (Exception ex)
            {
                await HandleErrorTenantCreatedAsync(eventData, ex);
            }
        }

        private async Task SeedDataAsync(Guid? tenantId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                {
                    var multiTenancySide = tenantId == null
                        ? MultiTenancySides.Host
                        : MultiTenancySides.Tenant;

                    var permissionNames = _permissionDefinitionManager
                        .GetPermissions()
                        .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                        .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePermissionValueProvider.ProviderName))
                        .Select(p => p.Name)
                        .ToArray();

                    await _permissionDataSeeder.SeedAsync(
                        RolePermissionValueProvider.ProviderName,
                        "admin",
                        permissionNames,
                        tenantId
                    );

                    await uow.CompleteAsync();
                }
            }
        }
    }
}
