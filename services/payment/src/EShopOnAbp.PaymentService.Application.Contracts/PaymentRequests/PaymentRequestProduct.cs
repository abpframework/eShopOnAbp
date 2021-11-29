using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    [Serializable]
    public class PaymentRequestProductDto : EntityDto<Guid>
    {
        public Guid PaymentRequestId { get; private set; }

        public string ReferenceId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

    }
}