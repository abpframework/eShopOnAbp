using System.Collections.Generic;

namespace EShopOnAbp.BasketService;

public class OrderAcceptedEto
{
    public List<OrderItemEto> Items { get; set; } 
}