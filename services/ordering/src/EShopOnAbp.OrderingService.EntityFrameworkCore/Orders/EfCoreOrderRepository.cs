using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.OrderingService.Orders;

public class EfCoreOrderRepository : EfCoreRepository<OrderingServiceDbContext, Order, Guid>, IOrderRepository
{
    public EfCoreOrderRepository(IDbContextProvider<OrderingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public override async Task<Order> InsertAsync(Order entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var newEntity = await base.InsertAsync(entity, autoSave, GetCancellationToken(cancellationToken));
        await EnsurePropertyLoadedAsync(newEntity, o => o.OrderStatus, GetCancellationToken(cancellationToken));
        await EnsurePropertyLoadedAsync(newEntity, o => o.PaymentType, GetCancellationToken(cancellationToken));
        return newEntity;
    }

    public override async Task<IQueryable<Order>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(q => q.OrderStatus)
            .Include(q => q.PaymentType);
    }
}