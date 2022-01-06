using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders;
using EShopOnAbp.OrderingService.Orders.Specifications;
using EShopOnAbp.OrderingService.Samples;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace EShopOnAbp.OrderingService.EntityFrameworkCore.Orders;

public class OrderRepository_Tests : SampleRepository_Tests<OrderingServiceEntityFrameworkCoreTestModule>
{
    private readonly TestData _testData;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderingServiceDbContext _dbContext;

    public OrderRepository_Tests()
    {
        _dbContext = GetRequiredService<IOrderingServiceDbContext>();
        _orderRepository = GetRequiredService<IOrderRepository>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task Should_Get_OrderStatus()
    {
        var dbSet = _dbContext.Set<OrderStatus>();
        var statusList = await dbSet.ToListAsync();
        statusList.Count.ShouldNotBe(0);
    }
    
    [Fact]
    public async Task Should_Get_User_Orders()
    {
        var orders =
            await _orderRepository.GetOrdersByUserId(_testData.CurrentUserId, new Last30DaysSpecification(), true);
        orders.Count.ShouldBe(3);
        var firstOrder = orders.First();
        firstOrder.OrderItems.Count.ShouldBe(5);
    }
    [Fact]
    public async Task Should_Get_Users_Last_Year_Orders()
    {
        var orders =
            await _orderRepository.GetOrdersByUserId(_testData.CurrentUserId, new YearSpecification(2020), true);
        orders.Count.ShouldBe(0);
    }
}