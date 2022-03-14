using EShopOnAbp.AdministrationService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.AdministrationService.DbMigrations;

public class AdministrationServiceDatabaseMigrationEventHandler
    : DatabaseEfCoreMigrationEventHandler<AdministrationServiceDbContext>,
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
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock distributedLockProvider
    ) : base(
        currentTenant,
        unitOfWorkManager,
        tenantStore,
        distributedEventBus,
        AdministrationServiceDbProperties.ConnectionStringName,
        distributedLockProvider
    )
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
            await using (var handle = await DistributedLockProvider.TryAcquireAsync(DatabaseName))
            {
                Log.Information("AdministrationService acquired lock for db migration and seeding...");

                if (handle != null)
                {
                    await MigrateDatabaseSchemaAsync();
                    await SeedDataAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
        }
    }

    private async Task SeedDataAsync()
    {
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var multiTenancySide = MultiTenancySides.Host;

            var permissionNames = _permissionDefinitionManager
                .GetPermissions()
                .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                .Where(p => !p.Providers.Any() ||
                            p.Providers.Contains(RolePermissionValueProvider.ProviderName))
                .Select(p => p.Name)
                .ToArray();

            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                permissionNames
            );

            await uow.CompleteAsync();
        }
    }
}