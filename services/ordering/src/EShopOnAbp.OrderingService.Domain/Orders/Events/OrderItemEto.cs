using System;

namespace EShopOnAbp.OrderingService.Orders.Events
{
    public class OrderItemEto
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}
