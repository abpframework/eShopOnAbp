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
    }

    private async Task SeedOrderStatusAsync()
    {
        if (!await _dbContext.Set<OrderStatus>().AnyAsync())
        {
            _dbContext.Set<OrderStatus>().AddRange(GetOrderStatusList());
            await _dbContext.SaveChangesAsync();
        }
    }

    private List<OrderStatus> GetOrderStatusList()
    {
        return new List<OrderStatus>
        {
            OrderStatus.Submitted,
            OrderStatus.AwaitingValidation,
            OrderStatus.StockConfirmed,
            OrderStatus.Paid,
            OrderStatus.Shipped,
            OrderStatus.Cancelled
        };
    }
}