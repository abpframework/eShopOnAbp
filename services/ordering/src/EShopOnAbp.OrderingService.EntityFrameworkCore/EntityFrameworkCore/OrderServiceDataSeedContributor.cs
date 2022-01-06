using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.OrderingService.EntityFrameworkCore;

/// <summary>
/// DataSeedContributor for seeding pre-exist order status data.
/// This is a sample for seeding data without DbSet (non-aggregate data), without using repository.
/// </summary>
public class OrderServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IOrderingServiceDbContext _dbContext;

    public OrderServiceDataSeedContributor(IOrderingServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedOrderStatusAsync();
        await SeedPaymentTypesAsync();
    }

    private async Task SeedPaymentTypesAsync()
    {
        if (!await _dbContext.Set<PaymentType>().AnyAsync())
        {
            await _dbContext.Set<PaymentType>().AddRangeAsync(PaymentType.List());
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedOrderStatusAsync()
    {
        if (!await _dbContext.Set<OrderStatus>().AnyAsync())
        {
            await _dbContext.Set<OrderStatus>().AddRangeAsync(OrderStatus.List());
            await _dbContext.SaveChangesAsync();
        }
    }
}