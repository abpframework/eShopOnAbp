using System;
using Volo.Abp.Domain.Repositories;

namespace EShopOnAbp.OrderingService.Orders;

public interface IOrderRepository : IRepository<Order, Guid>
{
}