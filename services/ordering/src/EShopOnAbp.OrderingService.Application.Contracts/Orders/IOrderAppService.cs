using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.OrderingService.Orders;

public interface IOrderAppService : IApplicationService
{
    Task<OrderDto> CreateAsync(OrderCreateDto input);
    Task<OrderDto> GetAsync(Guid id);
    Task<List<OrderDto>> GetMyOrdersAsync(GetMyOrdersInput input);
    Task<List<OrderDto>> GetOrdersAsync(GetOrdersInput input);
    Task<OrderDto> GetByOrderNoAsync(int orderNo);
}