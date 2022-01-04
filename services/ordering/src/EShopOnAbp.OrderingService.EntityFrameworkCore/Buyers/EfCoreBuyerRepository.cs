using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.OrderingService.Buyers;

public class EfCoreBuyerRepository : EfCoreRepository<OrderingServiceDbContext, Buyer, Guid>, IBuyerRepository
{
    public EfCoreBuyerRepository(IDbContextProvider<OrderingServiceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public override async Task<Buyer> InsertAsync(Buyer entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var newEntity = await base.InsertAsync(entity, autoSave, cancellationToken);
        await EnsurePropertyLoadedAsync(newEntity, query => query.PaymentType, cancellationToken);
        return newEntity;
    }

    public override async Task<IQueryable<Buyer>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(q => q.PaymentType);
    }
}