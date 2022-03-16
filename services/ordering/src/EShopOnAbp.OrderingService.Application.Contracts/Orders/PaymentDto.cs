using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.OrderingService.Orders;

public class PaymentDto : EntityDto<Guid>
{
    public int CountOfPaymentMethod { get; set; }
    public string PaymentMethod { get; set; }
}