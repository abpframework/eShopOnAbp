using System.Collections.Generic;

namespace EShopOnAbp.OrderingService.Orders;

public class OrderCreateDto
{
    public int PaymentTypeId { get; set; }
    public OrderAddressDto Address { get; set; } = new();
    public List<OrderItemCreateDto> Products { get; set; } = new();
}