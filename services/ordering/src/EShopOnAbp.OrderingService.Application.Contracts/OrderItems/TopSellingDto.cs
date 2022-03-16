using System;
using Volo.Abp.Application.Dtos;

namespace EShopOnAbp.OrderingService.OrderItems
{
    public class TopSellingDto : EntityDto<Guid>
    {
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public int Units { get; set; }
    }
}
