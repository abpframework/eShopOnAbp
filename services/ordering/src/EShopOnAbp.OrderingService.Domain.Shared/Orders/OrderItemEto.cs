using System;

namespace EShopOnAbp.OrderingService.Orders
{
    public class OrderItemEto
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}
