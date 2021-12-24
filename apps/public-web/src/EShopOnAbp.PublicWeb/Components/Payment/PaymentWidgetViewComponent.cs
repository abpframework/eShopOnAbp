using System.Threading.Tasks;
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
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View("~/Components/Payment/Default.cshtml"));
    }
}