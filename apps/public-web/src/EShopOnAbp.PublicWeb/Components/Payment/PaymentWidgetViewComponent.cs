using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.BasketService;
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
public class PaymentWidgetViewComponent : AbpViewComponent
{
    private readonly UserBasketProvider _userBasketProvider;
    private readonly UserAddressProvider _userAddressProvider;

    public PaymentWidgetViewComponent(UserBasketProvider userBasketProvider, UserAddressProvider userAddressProvider)
    {
        _userBasketProvider = userBasketProvider;
        _userAddressProvider = userAddressProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new PaymentViewModel()
        {
            Basket = await _userBasketProvider.GetBasketAsync(),
            Address = _userAddressProvider.GetDemoAddresses()
        };
        return View("~/Components/Payment/Default.cshtml", viewModel);
    }
}

public class PaymentViewModel
{
    public BasketDto Basket { get; set; }
    public List<AddressDto> Address { get; set; }
}