using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.OrderingService.Buyers;

public class BuyerDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PaymentType { get; set; }
    public int PaymentTypeId { get; set; }
}