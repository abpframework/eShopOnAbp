using EShopOnAbp.OrderingService.EntityFrameworkCore;
using System;
using EShopOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.OrderingService.DbMigrations;

public class OrderingServiceDatabaseMigrationChecker 
    : PendingEfCoreMigrationsChecker<OrderingServiceDbContext>
{
    public OrderingServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            OrderingServiceDbProperties.ConnectionStringName)
    {
    }
}