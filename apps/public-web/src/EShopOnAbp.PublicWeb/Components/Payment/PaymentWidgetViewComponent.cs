using System.Threading.Tasks;
using EShopOnAbp.PublicWeb.Basket;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EShopOnAbp.PublicWeb.Components.Payment;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/Payment",
    StyleTypes = new[] {typeof(PaymentWidgetStyleContributor)},
    ScriptTypes = new[] {typeof(PaymentWidgetScriptContributor)}
)]
public class PaymentWidgetViewComponent: AbpViewComponent
{
    private readonly UserBasketProvider _userBasketProvider;

    public PaymentWidgetViewComponent(UserBasketProvider userBasketProvider)
    {
        _userBasketProvider = userBasketProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Components/Payment/Default.cshtml", await _userBasketProvider.GetBasketAsync());
    }
}