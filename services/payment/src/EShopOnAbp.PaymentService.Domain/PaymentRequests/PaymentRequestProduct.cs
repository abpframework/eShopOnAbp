using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public class PaymentRequestProduct : Entity<Guid>
    {
        public Guid PaymentRequestId { get; private set; }

        public string ReferenceId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public PaymentRequestProduct(
            Guid id,
            Guid paymentRequestId,
            [NotNull] string name,
            decimal unitPrice,
            int quantity,
            decimal totalPrice,
            [CanBeNull] string referenceId = null)
        {
            Id = id;
            PaymentRequestId = paymentRequestId;
            Name = name;
            UnitPrice = unitPrice;
            Quantity = quantity;
            TotalPrice = totalPrice;
            ReferenceId = referenceId;
        }
    }
}
