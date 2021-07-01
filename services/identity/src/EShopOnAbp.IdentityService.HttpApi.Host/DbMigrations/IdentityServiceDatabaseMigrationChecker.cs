using EShopOnAbp.IdentityService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;
using System;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class IdentityServiceDatabaseMigrationChecker : PendingMigrationsCheckerBase<IdentityServiceDbContext>
    {
        protected ILocalEventBus LocalEventBus { get; }

        public IdentityServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus,
            ILocalEventBus localEventBus)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                IdentityServiceDbProperties.ConnectionStringName)
        {
            LocalEventBus = localEventBus;
        }


        public override async Task<bool> CheckAsync()
        {
            var isMigrationRequired = await base.CheckAsync();

            if (!isMigrationRequired)
            {
                await LocalEventBus.PublishAsync(new ApplyDatabaseSeedsEto());
            }

            return isMigrationRequired;
        }
    }
}