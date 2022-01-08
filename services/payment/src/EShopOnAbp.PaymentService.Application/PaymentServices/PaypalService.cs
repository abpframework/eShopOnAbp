using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentRequests;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentServices;

[ExposeServices(typeof(IPaymentStrategy), typeof(PaypalService))]
public class PaypalService : IPaymentStrategy
{
    private readonly PayPalHttpClient _payPalHttpClient;
    private readonly PaymentRequestDomainService _paymentRequestDomainService;

    public PaypalService(PayPalHttpClient payPalHttpClient, PaymentRequestDomainService paymentRequestDomainService)
    {
        _payPalHttpClient = payPalHttpClient;
        _paymentRequestDomainService = paymentRequestDomainService;
    }

    public async Task<PaymentRequestStartResultDto> StartAsync(PaymentRequest paymentRequest,
        PaymentRequestStartDto input)
    {
        var totalCheckoutPrice = paymentRequest.Products.Sum(s => s.TotalPrice);

        var order = new OrderRequest
        {
            CheckoutPaymentIntent = "CAPTURE",
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = input.ReturnUrl,
                CancelUrl = input.CancelUrl,
            },
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        AmountBreakdown = new AmountBreakdown
                        {
                            ItemTotal = new Money
                            {
                                CurrencyCode = paymentRequest.Currency,
                                Value = totalCheckoutPrice.ToString(
                                    $"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00")
                            }
                        },
                        CurrencyCode = paymentRequest.Currency,
                        Value = totalCheckoutPrice.ToString(
                            $"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00"),
                    },
                    Items = paymentRequest.Products.Select(p => new Item
                    {
                        Quantity = p.Quantity.ToString(),
                        Name = p.Name,
                        UnitAmount = new Money
                        {
                            CurrencyCode = paymentRequest.Currency,
                            Value = p.UnitPrice.ToString(
                                $"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00")
                        }
                    }).ToList(),
                    ReferenceId = paymentRequest.Id.ToString()
                }
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);

        Order result = (await _payPalHttpClient.Execute(request)).Result<Order>();

        return new PaymentRequestStartResultDto
        {
            CheckoutLink = result.Links.First(x => x.Rel == "approve").Href
        };
    }

    public async Task<PaymentRequest> CompleteAsync(IPaymentRequestRepository paymentRequestRepository, string token)
    {
        var request = new OrdersCaptureRequest(token);
        request.RequestBody(new OrderActionRequest());

        var order = (await _payPalHttpClient.Execute(request)).Result<Order>();

        var paymentRequestId = Guid.Parse(order.PurchaseUnits.First().ReferenceId);
        return await _paymentRequestDomainService.UpdatePaymentRequestStateAsync(paymentRequestId, order.Status,
            order.Id);
    }
}