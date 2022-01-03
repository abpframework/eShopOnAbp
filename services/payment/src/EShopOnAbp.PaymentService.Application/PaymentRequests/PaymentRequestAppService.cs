using EShopOnAbp.PaymentService.PayPal;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    [ExposeServices(typeof(PaymentRequestAppService))]
    public class PaymentRequestAppService : PaymentServiceAppService, IPaymentRequestAppService
    {
        protected IPaymentRequestRepository PaymentRequestRepository { get; }
        protected PayPalHttpClient PayPalHttpClient { get; }

        public PaymentRequestAppService(
            IPaymentRequestRepository paymentRequestRepository,
            PayPalHttpClient payPalHttpClient)
        {
            PaymentRequestRepository = paymentRequestRepository;
            PayPalHttpClient = payPalHttpClient;
        }

        public virtual async Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            var paymentRequest = new PaymentRequest(id: GuidGenerator.Create(), orderId: input.OrderId, currency: input.Currency, buyerId: input.BuyerId);

            foreach (var paymentRequestProduct in input.Products
                .Select(s => new PaymentRequestProduct(
                    GuidGenerator.Create(),
                    paymentRequestId: paymentRequest.Id,
                    code: s.Code,
                    name: s.Name,
                    unitPrice: s.UnitPrice,
                    quantity: s.Quantity,
                    totalPrice: s.TotalPrice,
                    referenceId: s.ReferenceId)))
            {
                paymentRequest.Products.Add(paymentRequestProduct);
            }

            await PaymentRequestRepository.InsertAsync(paymentRequest);

            return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);
        }

        public virtual async Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input)
        {
            var paymentRequest = await PaymentRequestRepository.GetAsync(input.PaymentRequestId, includeDetails: true);

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
                                    Value = totalCheckoutPrice.ToString($"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00")
                                }
                            },
                            CurrencyCode = paymentRequest.Currency,
                            Value = totalCheckoutPrice.ToString($"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00"),
                        },
                        Items = paymentRequest.Products.Select(p => new Item
                        {
                            Quantity = p.Quantity.ToString(),
                            Name = p.Name,
                            UnitAmount = new Money
                            {
                                CurrencyCode = paymentRequest.Currency,
                                Value = p.UnitPrice.ToString($"{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00")
                            }
                        }).ToList(),
                        ReferenceId = paymentRequest.Id.ToString()
                    }
                }
            };

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(order);

            var result = (await PayPalHttpClient.Execute(request)).Result<Order>();

            return new PaymentRequestStartResultDto
            {
                CheckoutLink = result.Links.First(x => x.Rel == "approve").Href
            };
        }

        public virtual async Task<PaymentRequestDto> CompleteAsync(string token)
        {
            var request = new OrdersCaptureRequest(token);
            request.RequestBody(new OrderActionRequest());

            var order = (await PayPalHttpClient.Execute(request)).Result<Order>();

            var paymentRequest = await UpdatePaymentRequestStateAsync(order);

            return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);
        }

        public virtual async Task<bool> HandleWebhookAsync(string payload)
        {
            var jObject = JObject.Parse(payload);

            var order = jObject["resource"].ToObject<Order>();

            var request = new OrdersGetRequest(order.Id);

            // Ensure order object comes from PayPal
            var response = await PayPalHttpClient.Execute(request);
            order = response.Result<Order>();

            await UpdatePaymentRequestStateAsync(order);

            // PayPal doesn't accept Http 204 (NoContent) result and tries to execute webhook again.
            // So with following value, API returns Http 200 (OK) result.
            return true;
        }

        protected async Task<PaymentRequest> UpdatePaymentRequestStateAsync(Order order)
        {
            var paymentRequestId = Guid.Parse(order.PurchaseUnits.First().ReferenceId);

            var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);

            if (order.Status == PayPalConsts.OrderStatus.Completed || order.Status == PayPalConsts.OrderStatus.Approved)
            {
                paymentRequest.SetAsCompleted();
            }
            else
            {
                paymentRequest.SetAsFailed(order.Status);
            }

            paymentRequest.ExtraProperties[PayPalConsts.OrderIdPropertyName] = order.Id;
            paymentRequest.ExtraProperties[nameof(order.Status)] = order.Status;

            await PaymentRequestRepository.UpdateAsync(paymentRequest);

            return paymentRequest;
        }
    }
}
