using EShopOnAbp.PaymentService.EntityFrameworkCore;
using System;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.PaymentService.DbMigrations
{
    public class PaymentServiceDatabaseMigrationChecker : PendingEfCoreMigrationsChecker<PaymentServiceDbContext>
    {
        public PaymentServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus)
            : base(
                unitOfWorkManager,
                serviceProvider,
                currentTenant,
                distributedEventBus,
                PaymentServiceDbProperties.ConnectionStringName)
        {

        }
    }
}
