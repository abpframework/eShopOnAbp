using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Cart;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/Cart",
    StyleFiles = new[] {"/components/cart/cart-widget.css"},
    ScriptTypes = new[] {typeof(CartWidgetScriptContributor)}
)]
public class CartWidgetViewComponent : AbpViewComponent
{
    private readonly IBasketAppService _basketAppService;

    public CartWidgetViewComponent(IBasketAppService basketAppService)
    {
        _basketAppService = basketAppService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basketDto = await _basketAppService.GetAsync();
        return View("~/Components/Toolbar/Cart/Default.cshtml", basketDto);
    }
}