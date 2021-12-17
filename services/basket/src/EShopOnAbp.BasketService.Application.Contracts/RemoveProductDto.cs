using System;

namespace EShopOnAbp.BasketService;

public class RemoveProductDto
{
    public Guid ProductId { get; set; }

    public int? Count { get; set; }
}