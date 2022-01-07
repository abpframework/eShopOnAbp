using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages;

[Authorize]
public class PaymentCompletedModel : AbpPageModel
{
    private readonly IPaymentRequestAppService _paymentRequestAppService;

    public PaymentCompletedModel(IPaymentRequestAppService paymentRequestAppService)
    {
        _paymentRequestAppService = paymentRequestAppService;
    }

    [BindProperty(SupportsGet = true)] public string Token { get; set; }

    public PaymentRequestDto PaymentRequest { get; private set; }

    public bool IsSuccessful { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        PaymentRequest = await _paymentRequestAppService.CompleteAsync(Token);

        IsSuccessful = PaymentRequest.State == PaymentRequestState.Completed;
        if (IsSuccessful)
        {
            return RedirectToPage("OrderReceived", new { orderNo = PaymentRequest.OrderNo });    
        }

        return Page();
    }
}