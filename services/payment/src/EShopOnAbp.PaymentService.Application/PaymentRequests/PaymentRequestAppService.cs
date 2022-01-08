using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentServices;
using Microsoft.Extensions.Logging;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public class PaymentRequestAppService : PaymentServiceAppService, IPaymentRequestAppService
    {
        private readonly PaymentServiceFactory _paymentServiceFactory;
        private readonly PaymentRequestDomainService _paymentRequestDomainService;
        protected IPaymentRequestRepository PaymentRequestRepository { get; }
        protected PayPalHttpClient PayPalHttpClient { get; }


        public PaymentRequestAppService(
            IPaymentRequestRepository paymentRequestRepository,
            PayPalHttpClient payPalHttpClient,
            PaymentServiceFactory paymentServiceFactory,
            PaymentRequestDomainService paymentRequestDomainService)
        {
            PaymentRequestRepository = paymentRequestRepository;
            PayPalHttpClient = payPalHttpClient;
            _paymentServiceFactory = paymentServiceFactory;
            _paymentRequestDomainService = paymentRequestDomainService;
        }

        public virtual async Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            var paymentRequest = new PaymentRequest(id: GuidGenerator.Create(), orderId: input.OrderId,
                orderNo: input.OrderNo, currency: input.Currency, buyerId: input.BuyerId);

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
            PaymentRequest paymentRequest = await PaymentRequestRepository.GetAsync(input.PaymentRequestId, includeDetails: true);
            
            var paymentService = _paymentServiceFactory.Create(input.PaymentTypeId);
            return await paymentService.StartAsync(paymentRequest, input);
        }

        public virtual async Task<PaymentRequestDto> CompleteAsync(PaymentRequestCompleteInputDto input)
        {
            var paymentService = _paymentServiceFactory.Create(input.PaymentTypeId);
            
            var paymentRequest = await paymentService.CompleteAsync(PaymentRequestRepository, input.Token);
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

            var paymentRequestId = Guid.Parse(order.PurchaseUnits.First().ReferenceId);
            await _paymentRequestDomainService.UpdatePaymentRequestStateAsync(paymentRequestId, order.Status, order.Id);

            // PayPal doesn't accept Http 204 (NoContent) result and tries to execute webhook again.
            // So with following value, API returns Http 200 (OK) result.
            return true;
        }
    }
}