using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace EShopOnAbp.PaymentService
{
    [Serializable]
    [EventName("Payment.Completed")]
    public class PaymentRequestFailedEto : EtoBase, IHasExtraProperties
    {
        public Guid PaymentRequestId { get; set; }
        public string FailReason { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}