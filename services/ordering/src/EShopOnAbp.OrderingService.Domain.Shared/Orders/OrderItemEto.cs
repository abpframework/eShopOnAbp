using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnAbp.OrderingService.Orders
{
    public class OrderItemEto
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}
