using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShopOnAbp.CatalogService.Products;

namespace EShopOnAbp.PublicWeb.Pages;

public class Catalog : PageModel
{
    public IReadOnlyList<ProductDto> Products { get; private set; }
    private readonly IPublicProductAppService _productAppService;

    public Catalog(IPublicProductAppService productAppService)
    {
        _productAppService = productAppService;
    }
    
    public async Task OnGetAsync()
    {
        Products = (await _productAppService.GetListAsync()).Items;
    }
}