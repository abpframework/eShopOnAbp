using System;
using System.Collections.Generic;
using EShopOnAbp.OrderingService.Buyers;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace EShopOnAbp.OrderingService.Orders;

[EventName("EShopOnAbp.Order.Placed")]
public class OrderPlacedEto : EtoBase
{
    public Guid OrderId { get; set; }
    public BuyerEto Buyer { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemEto> Items { get; set; } = new();
}