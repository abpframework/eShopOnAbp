using System;
using EShopOnAbp.OrderingService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.OrderingService.Buyers;

public class EfCoreBuyerRepository : EfCoreRepository<OrderingServiceDbContext, Buyer, Guid>, IBuyerRepository
{
    public EfCoreBuyerRepository(IDbContextProvider<OrderingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}