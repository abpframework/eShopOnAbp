using EShopOnAbp.BasketService;
using EShopOnAbp.PaymentService;
using EShopOnAbp.PaymentService.PaymentRequests;
using EShopOnAbp.PublicWeb.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Pages;

[Authorize]
public class PaymentModel : AbpPageModel
{
    private readonly IPaymentRequestAppService _paymentRequestAppService;
    private readonly UserBasketProvider _userBasketProvider;
    private readonly EShopOnAbpPublicWebPaymentOptions _publicWebPaymentOptions;

    public PaymentModel(
        IPaymentRequestAppService paymentRequestAppService,
        UserBasketProvider userBasketProvider,
        IOptions<EShopOnAbpPublicWebPaymentOptions> publicWebPaymentOptions)
    {
        _paymentRequestAppService = paymentRequestAppService;
        _userBasketProvider = userBasketProvider;
        _publicWebPaymentOptions = publicWebPaymentOptions.Value;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Logger.LogInformation("Payment Proceeded...");

        var basket = await _userBasketProvider.GetBasketAsync();

        var paymentRequest = await _paymentRequestAppService.CreateAsync(new PaymentRequestCreationDto
        {
            BuyerId = CurrentUser.GetId().ToString(),
            Currency = EShopOnAbpPaymentConsts.Currency,
            Products = ObjectMapper.Map<List<BasketItemDto>, List<PaymentRequestProductCreationDto>>(basket.Items)
        });

        var response = await _paymentRequestAppService.StartAsync(new PaymentRequestStartDto
        {
            ReturnUrl = _publicWebPaymentOptions.PaymentSuccessfulCallbackUrl,
            PaymentRequestId = paymentRequest.Id,
            CancelUrl = _publicWebPaymentOptions.PaymentFailureCallbackUrl,
        });

        return Redirect(response.CheckoutLink);
    }
}