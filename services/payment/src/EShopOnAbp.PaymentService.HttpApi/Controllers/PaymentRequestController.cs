using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace EShopOnAbp.PaymentService.Controllers
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Area("payment")]
    [Route("api/payment/requests")]
    public class PaymentRequestController : PaymentServiceController, IPaymentRequestAppService
    {
        protected IPaymentRequestAppService PaymentRequestAppService { get; }

        public PaymentRequestController(IPaymentRequestAppService paymentRequestAppService)
        {
            PaymentRequestAppService = paymentRequestAppService;
        }

        [HttpPost("{paymentType}/complete")]
        public Task<PaymentRequestDto> CompleteAsync(string paymentType, PaymentRequestCompleteInputDto input)
        {
            return PaymentRequestAppService.CompleteAsync(paymentType, input);
        }

        [HttpPost]
        public Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            return PaymentRequestAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("{paymentType}/webhook")]
        public async Task<bool> HandleWebhookAsync(string paymentType, string payload)
        {
            var bytes = await Request.Body.GetAllBytesAsync();
            payload = Encoding.UTF8.GetString(bytes);

            return await PaymentRequestAppService.HandleWebhookAsync(paymentType, payload);
        }

        [HttpPost("{paymentType}/start")]
        public Task<PaymentRequestStartResultDto> StartAsync(string paymentType, PaymentRequestStartDto input)
        {
            return PaymentRequestAppService.StartAsync(paymentType, input);
        }
    }
}