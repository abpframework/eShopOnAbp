using System;
using Volo.Abp.Domain.Repositories;

namespace EShopOnAbp.OrderingService.Buyers;

public interface IBuyerRepository : IRepository<Buyer, Guid>
{
}