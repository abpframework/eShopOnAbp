using System;
using System.ComponentModel.DataAnnotations;

namespace EShopOnAbp.BasketService;

public class AddProductDto
{
    public Guid ProductId { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Count { get; set; } = 1;
}