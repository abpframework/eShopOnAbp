using System;

namespace EShopOnAbp.BasketService;

public class OrderItemEto
{
    public Guid ProductId { get; set; }
    public int Count { get; set; }
}