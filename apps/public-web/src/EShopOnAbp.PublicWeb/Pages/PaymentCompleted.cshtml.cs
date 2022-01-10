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
        int selectedPaymentId = 0;
        if (HttpContext.Request.Cookies.TryGetValue(EShopOnAbpPaymentConsts.PaymentIdCookie,
                out var selectedPaymentIdString))
        {
            selectedPaymentId = string.IsNullOrEmpty(selectedPaymentIdString) ? 0 : int.Parse(selectedPaymentIdString);
        }

        PaymentRequest = await _paymentRequestAppService.CompleteAsync(
            new PaymentRequestCompleteInputDto() {Token = Token, PaymentTypeId = selectedPaymentId});

        IsSuccessful = PaymentRequest.State == PaymentRequestState.Completed;
        
        if (IsSuccessful)
        {
            // Remove cookie so that can be set again when default payment type is set
            HttpContext.Response.Cookies.Delete(EShopOnAbpPaymentConsts.PaymentIdCookie);
            return RedirectToPage("OrderReceived", new {orderNo = PaymentRequest.OrderNo});
        }

        return Page();
    }
}