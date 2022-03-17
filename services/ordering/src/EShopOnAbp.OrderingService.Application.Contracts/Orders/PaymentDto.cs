using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.OrderingService.Orders;

public class PaymentDto : EntityDto<Guid>
{
    public decimal RateOfPaymentMethod { get; set; }
    public string PaymentMethod { get; set; }
}