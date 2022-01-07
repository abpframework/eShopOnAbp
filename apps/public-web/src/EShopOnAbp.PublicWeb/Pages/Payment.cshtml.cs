using EShopOnAbp.BasketService;
using EShopOnAbp.PaymentService.PaymentRequests;
using EShopOnAbp.PublicWeb.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Pages;

[Authorize]
public class PaymentModel : AbpPageModel
{
    private readonly IPaymentRequestAppService _paymentRequestAppService;
    private readonly IOrderAppService _orderAppService;
    private readonly UserBasketProvider _userBasketProvider;
    private readonly UserAddressProvider _userAddressProvider;
    private readonly EShopOnAbpPublicWebPaymentOptions _publicWebPaymentOptions;

    public PaymentModel(
        IPaymentRequestAppService paymentRequestAppService,
        IOrderAppService orderAppService,
        UserBasketProvider userBasketProvider,
        UserAddressProvider userAddressProvider,
        IOptions<EShopOnAbpPublicWebPaymentOptions> publicWebPaymentOptions)
    {
        _paymentRequestAppService = paymentRequestAppService;
        _userBasketProvider = userBasketProvider;
        _userAddressProvider = userAddressProvider;
        _orderAppService = orderAppService;
        _publicWebPaymentOptions = publicWebPaymentOptions.Value;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(PaymentPageViewModel model)
    {
        Logger.LogInformation("Payment Proceeded...");
        Logger.LogInformation($"AddressId: {model.SelectedAddressId}");
        Logger.LogInformation($"PaymentId: {model.SelectedPaymentId}");

        var basket = await _userBasketProvider.GetBasketAsync();
        var placedOrder = await _orderAppService.CreateAsync(new OrderCreateDto()
        {
            PaymentTypeId = 1, // Paypal
            Address = GetUserAddress(model.SelectedAddressId),
            Products = ObjectMapper.Map<List<BasketItemDto>, List<OrderItemCreateDto>>(basket.Items)
        });

        var paymentRequest = await _paymentRequestAppService.CreateAsync(new PaymentRequestCreationDto
        {
            OrderId = placedOrder.Id.ToString(),
            OrderNo = placedOrder.OrderNo,
            BuyerId = CurrentUser.GetId().ToString(),
            Currency = EShopOnAbpPaymentConsts.Currency,
            Products = ObjectMapper.Map<List<BasketItemDto>, List<PaymentRequestProductCreationDto>>(basket.Items)
        });

        var response = await _paymentRequestAppService.StartAsync(new PaymentRequestStartDto
        {
            PaymentRequestId = paymentRequest.Id,
            ReturnUrl = _publicWebPaymentOptions.PaymentSuccessfulCallbackUrl,
            CancelUrl = _publicWebPaymentOptions.PaymentFailureCallbackUrl,
        });

        return Redirect(response.CheckoutLink);
    }

    private OrderAddressDto GetUserAddress(int selectedAddressId)
    {
        var address = _userAddressProvider.GetDemoAddresses().First(q => q.Id == selectedAddressId);
        return new OrderAddressDto
        {
            City = address.City,
            Country = address.Country,
            Description = address.Type,
            Street = address.Street,
            ZipCode = address.ZipCode
        };
    }

    public class PaymentPageViewModel
    {
        public int SelectedAddressId { get; set; }
        public int SelectedPaymentId { get; set; }
    }
}