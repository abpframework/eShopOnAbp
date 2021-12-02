using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.Shared.Hosting.Microservices.DbMigrations;

public class PendingMongoDbMigrationsChecker<TDbContext> : PendingMigrationsCheckerBase
    where TDbContext : AbpMongoDbContext
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected string DatabaseName { get; }

    protected PendingMongoDbMigrationsChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        string databaseName)
    {
        UnitOfWorkManager = unitOfWorkManager;
        ServiceProvider = serviceProvider;
        CurrentTenant = currentTenant;
        DistributedEventBus = distributedEventBus;
        DatabaseName = databaseName;
    }

    public virtual async Task CheckAsync()
    {
        using (CurrentTenant.Change(null))
        {
            // Create database tables if needed
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
            {
                await DistributedEventBus.PublishAsync(
                    new ApplyDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName
                    }
                );

                await uow.CompleteAsync();
            }
        }
    }
}