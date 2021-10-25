using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Area("payment")]
    [Route("api/payment/requests")]

    public class PaymentRequestController : PaymentServiceController, IPaymentRequestAppService
    {
        protected IPaymentRequestAppService PaymentRequestAppService { get; }
        public PaymentOptions Options { get; }
        public IConfiguration Configuration { get; }

        public PaymentRequestController(
            IPaymentRequestAppService paymentRequestAppService,
            IOptions<PaymentOptions> options,
            IConfiguration configuration)
        {
            PaymentRequestAppService = paymentRequestAppService;
            Options = options.Value;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("complete")]
        public Task<PaymentRequestDto> CompleteAsync(string token)
        {
            return PaymentRequestAppService.CompleteAsync(token);
        }

        [HttpPost]
        public Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input)
        {
            return PaymentRequestAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("mock-token")]
        public Task<string> CreateMockTokenAsync(Guid paymentRequestId, bool successfulResult = true)
        {
            return PaymentRequestAppService.CreateMockTokenAsync(paymentRequestId, successfulResult);
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<bool> HandleWebhookAsync(string payload)
        {
            var bytes = await Request.Body.GetAllBytesAsync();
            payload = Encoding.UTF8.GetString(bytes);

            return await PaymentRequestAppService.HandleWebhookAsync(payload);
        }

        [HttpPost]
        [Route("start")]
        public async Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input)
        {
            if (Options.UseMock)
            {
                var token = await CreateMockTokenAsync(input.PaymentRequestId, true);
                var uri = new Uri(input.ReturnUrl);
                
                return new PaymentRequestStartResultDto
                {
                    CheckoutLink = .EndsWith("/") + $"?token={token}&returnUrl={input.ReturnUrl}"
                };
            }

            return await PaymentRequestAppService.StartAsync(input);
        }
    }
}
