using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace EShopOnAbp.OrderingService.Orders.Events
{
    [Serializable]
    [EventName("EShopOnAbp.Order.Accepted")]
    public class OrderAcceptedEto : EtoBase
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public List<OrderItemEto> Items { get; set; }
    }
}
