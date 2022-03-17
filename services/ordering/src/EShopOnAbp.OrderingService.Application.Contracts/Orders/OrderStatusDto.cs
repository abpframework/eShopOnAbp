using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.OrderingService.Orders;

public class OrderStatusDto : EntityDto<Guid>
{
    public int CountOfStatusOrder { get; set; }
    public string OrderStatus { get; set; }
    public int OrderStatusId { get; set; }
    public decimal TotalIncome{ get; set; }
}