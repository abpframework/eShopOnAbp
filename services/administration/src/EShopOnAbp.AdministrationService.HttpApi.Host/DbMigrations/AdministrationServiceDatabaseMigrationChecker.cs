using EShopOnAbp.AdministrationService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using System;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.AdministrationService.DbMigrations
{
    public class AdministrationServiceDatabaseMigrationChecker : PendingMigrationsCheckerBase<AdministrationServiceDbContext>
    {
        public AdministrationServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                AdministrationServiceDbProperties.ConnectionStringName)
        {

        }
    }
}
