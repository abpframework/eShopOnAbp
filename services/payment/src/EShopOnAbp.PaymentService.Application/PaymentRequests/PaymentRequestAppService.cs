using EShopOnAbp.PaymentService.PayPal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public class PaymentRequestAppService : PaymentServiceAppService, IPaymentRequestAppService
    {
        private const string MockFailureToken = "55F97E93";
        private const string MockSuccessToken = "A8195146CA5A";
        protected IPaymentRequestRepository PaymentRequestRepository { get; }
        protected PayPalHttpClient PayPalHttpClient { get; }
        public PaymentOptions Options { get; }

        public PaymentRequestAppService(
            IPaymentRequestRepository paymentRequestRepository,
            PayPalHttpClient payPalHttpClient,
            IOptions<PaymentOptions> options)
        {
            PaymentRequestRepository = paymentRequestRepository;
            PayPalHttpClient = payPalHttpClient;
            Options = options.Value;
        }

        public async Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            var paymentRequest = new PaymentRequest(GuidGenerator.Create(), input.Currency, input.BuyerId);

            foreach (var paymentRequestProduct in input.Products
                .Select(s => new PaymentRequestProduct(
                    paymentRequest.Id,
                    s.Name,
                    s.UnitPrice,
                    s.Quantity,
                    s.TotalPrice,
                    s.ReferenceId)))
            {
                paymentRequest.Products.Add(paymentRequestProduct);
            }

            return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);
        }

        public async Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input)
        {
            if (Options.UseMock)
            {
                return new PaymentRequestStartResultDto
                {
                    CheckoutLink = String.Empty
                };
            }

            var paymentRequest = await PaymentRequestRepository.GetAsync(input.PaymentRequestId);

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
                                    Value = totalCheckoutPrice.ToString(".00")
                                }
                            },
                            CurrencyCode = paymentRequest.Currency,
                            Value = totalCheckoutPrice.ToString(".00"),
                        },
                        Items = paymentRequest.Products.Select(p => new Item
                        {
                            Quantity = p.Quantity.ToString(),
                            Name = p.Name,
                            UnitAmount = new Money
                            {
                                CurrencyCode = paymentRequest.Currency,
                                Value = p.UnitPrice.ToString(".00")
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

        public async Task<PaymentRequestDto> CompleteAsync(string token)
        {
            if (Options.UseMock)
            {
                return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(await HandleMockTokenAsync(token));
            }

            var request = new OrdersCaptureRequest(token);
            request.RequestBody(new OrderActionRequest());

            var order = (await PayPalHttpClient.Execute(request)).Result<Order>();

            var paymentRequest = await UpdatePaymentRequestStateAsync(order);

            return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);
        }

        public async Task<bool> HandleWebhookAsync(string payload)
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

        public Task<string> CreateMockTokenAsync(Guid paymentRequestId, bool successfulResult = true)
        {
            if (Options.UseMock)
            {
                throw new InvalidOperationException("Mock token can't be genetared. UseMock has to be set as true in PaymentOptions.");
            }

            if (successfulResult)
            {
                return Task.FromResult(MockSuccessToken + "_" + paymentRequestId);
            }
            else
            {
                return Task.FromResult(MockFailureToken + "-" + paymentRequestId);
            }
        }

        private async Task<PaymentRequest> UpdatePaymentRequestStateAsync(Order order)
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

        private async Task<PaymentRequest> HandleMockTokenAsync(string token)
        {
            var isPaymentSuccessful = token.Contains($"{MockSuccessToken}_") ? true : token.Contains($"{MockFailureToken}_") ? false : throw new UserFriendlyException("Token is invalid!");

            PaymentRequest paymentRequest = null;
            if (isPaymentSuccessful)
            {
                paymentRequest = await PaymentRequestRepository.GetAsync(
                   Guid.Parse(token.Replace($"{MockSuccessToken}_", string.Empty)));

                paymentRequest.SetAsCompleted();
            }
            else
            {
                paymentRequest = await PaymentRequestRepository.GetAsync(
                   Guid.Parse(token.Replace($"{MockFailureToken}_", string.Empty)));

                paymentRequest.SetAsFailed("This payment has been mocked to be failed");
            }

            return paymentRequest;
        }
    }
}
