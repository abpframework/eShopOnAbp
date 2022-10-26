using EShopOnAbp.CatalogService.Products;
using EShopOnAbp.OrderingService.Orders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages
{
    public class ProductDetailModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public int OrderNo { get; set; }

        public ProductDto Product { get; private set; }
        public bool IsPurschased { get; private set; }

        private readonly IPublicProductAppService _productAppService;
        private readonly IOrderAppService _orderAppService;

        public ProductDetailModel(
            IPublicProductAppService productAppService,
            IOrderAppService orderAppService)
        {
            _productAppService = productAppService;
            _orderAppService = orderAppService;
        }

        public async Task OnGet(Guid id)
        {
            Product = await _productAppService.GetAsync(id);
            
            try
            {
                IsPurschased = (await _orderAppService.GetMyOrdersAsync(new GetMyOrdersInput())).Any(p => p.Items.Any(p => p.ProductId == id));
            }
            catch (Exception e)
            {
                IsPurschased = false;
                Console.WriteLine(e);
            }
        }
    }
}