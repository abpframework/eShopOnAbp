using System;
using EShopOnAbp.IdentityService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class IdentityServiceDatabaseMigrationChecker : PendingMigrationsCheckerBase<IdentityServiceDbContext>
    {
        public IdentityServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                IdentityServiceDbProperties.ConnectionStringName)
        {
            
        }
    }
}