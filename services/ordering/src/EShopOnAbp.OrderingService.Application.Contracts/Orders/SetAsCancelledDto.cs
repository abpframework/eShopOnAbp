using System;

namespace EShopOnAbp.OrderingService.Orders
{
    public class SetAsCancelledDto
    {
        public Guid PaymentRequestId { get; set; }
        public string PaymentRequestStatus { get; set; }
    }
}