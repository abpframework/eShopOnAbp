using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace EShopOnAbp.OrderingService.Buyers;

public class BuyerEto : EtoBase
{
    public Guid BuyerId { get; set; }
    public string BuyerEmail { get; set; }
    public string PaymentType { get; set; }
    public int PaymentTypeId { get; set; }
    
}