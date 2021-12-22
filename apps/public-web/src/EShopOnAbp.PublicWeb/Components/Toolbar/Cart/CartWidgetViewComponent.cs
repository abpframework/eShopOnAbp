using System;
using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using EShopOnAbp.PublicWeb.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Cart;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/Cart",
    StyleTypes = new[] {typeof(CartWidgetStyleContributor)},
    ScriptTypes = new[] {typeof(CartWidgetScriptContributor)}
)]
public class CartWidgetViewComponent : AbpViewComponent
{
    private readonly UserBasketProvider userBasketProvider;

    public CartWidgetViewComponent(UserBasketProvider userBasketProvider)
    {
        this.userBasketProvider = userBasketProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(
            "~/Components/Toolbar/Cart/Default.cshtml",
            await userBasketProvider.GetBasketAsync());
    }
}