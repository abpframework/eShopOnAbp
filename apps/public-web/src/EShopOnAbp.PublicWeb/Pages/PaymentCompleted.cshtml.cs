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

    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    public PaymentRequestDto PaymentRequest { get; private set; }

    public bool IsSuccessful { get; private set; }

    public async Task OnGetAsync()
    {
        PaymentRequest = await _paymentRequestAppService.CompleteAsync(Token);

        IsSuccessful = PaymentRequest.State == PaymentRequestState.Completed;
    }
}