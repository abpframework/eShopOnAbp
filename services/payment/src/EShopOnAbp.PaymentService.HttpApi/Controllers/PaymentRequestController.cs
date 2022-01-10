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

        [HttpPost("complete")]
        public Task<PaymentRequestDto> CompleteAsync(PaymentRequestCompleteInputDto input)
        {
            return PaymentRequestAppService.CompleteAsync(input);
        }

        [HttpPost]
        public Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            return PaymentRequestAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<bool> HandleWebhookAsync(string payload)
        {
            var bytes = await Request.Body.GetAllBytesAsync();
            payload = Encoding.UTF8.GetString(bytes);

            return await PaymentRequestAppService.HandleWebhookAsync(payload);
        }

        [HttpPost("start")]
        public Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input)
        {
            return PaymentRequestAppService.StartAsync(input);
        }
    }
}