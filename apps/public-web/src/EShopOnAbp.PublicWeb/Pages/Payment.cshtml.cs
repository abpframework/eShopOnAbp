using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages;

[Authorize]
public class PaymentModel : AbpPageModel
{
    private readonly IBasketAppService _basketAppService;

    public PaymentModel(IBasketAppService basketAppService)
    {
        _basketAppService = basketAppService;
    }

    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Logger.LogInformation("Payment Proceeded...");
        await _basketAppService.PurchaseAsync();

        return RedirectToPage("PaymentCompleted");
    }
}