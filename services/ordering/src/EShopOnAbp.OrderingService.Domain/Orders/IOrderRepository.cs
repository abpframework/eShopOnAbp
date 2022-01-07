using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EShopOnAbp.OrderingService.Orders;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<Order> GetByOrderNoAsync(int orderNo,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);
}