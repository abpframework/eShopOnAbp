using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.Products;
using Microsoft.AspNetCore.Authentication;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages
{
    public class IndexModel : AbpPageModel
    {
        public IReadOnlyList<ProductDto> Products { get; private set; }
        private readonly IPublicProductAppService _productAppService;

        public IndexModel(IPublicProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGet()
        {
            Products = (await _productAppService.GetListAsync()).Items;
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}