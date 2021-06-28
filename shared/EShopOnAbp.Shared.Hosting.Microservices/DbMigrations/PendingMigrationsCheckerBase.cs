using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EShopOnAbp.Shared.Hosting.Microservices.DbMigrations
{
    // public abstract class PendingMigrationsCheckerBase<TDbContext> : ITransientDependency
    //     where TDbContext : DbContext
    // {
    //     protected IUnitOfWorkManager UnitOfWorkManager { get; }
    //     protected IServiceProvider ServiceProvider { get; }
    //     protected ICurrentTenant CurrentTenant { get; }
    //     protected IDistributedEventBus DistributedEventBus { get; }
    //     protected string DatabaseName { get; }
    //
    //     protected PendingMigrationsCheckerBase(
    //         IUnitOfWorkManager unitOfWorkManager,
    //         IServiceProvider serviceProvider,
    //         ICurrentTenant currentTenant,
    //         IDistributedEventBus distributedEventBus,
    //         string databaseName)
    //     {
    //         UnitOfWorkManager = unitOfWorkManager;
    //         ServiceProvider = serviceProvider;
    //         CurrentTenant = currentTenant;
    //         DistributedEventBus = distributedEventBus;
    //         DatabaseName = databaseName;
    //     }
    //
    //     public virtual async Task CheckAsync()
    //     {
    //         using (CurrentTenant.Change(null))
    //         {
    //             // Create database tables if needed
    //             using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
    //             {
    //                 var pendingMigrations = await ServiceProvider
    //                     .GetRequiredService<TDbContext>()
    //                     .Database
    //                     .GetPendingMigrationsAsync();
    //
    //                 if (pendingMigrations.Any())
    //                 {
    //                     await DistributedEventBus.PublishAsync(
    //                         new ApplyDatabaseMigrationsEto
    //                         {
    //                             DatabaseName = DatabaseName
    //                         }
    //                     );
    //                 }
    //
    //                 await uow.CompleteAsync();
    //             }
    //         }
    //     }
    // }
}