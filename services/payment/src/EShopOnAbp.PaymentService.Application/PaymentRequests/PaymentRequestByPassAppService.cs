using PayPalCheckoutSdk.Core;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    [ExposeServices(typeof(PaymentRequestByPassAppService))]
    public class PaymentRequestByPassAppService : PaymentRequestAppService
    {
        public PaymentRequestByPassAppService(IPaymentRequestRepository paymentRequestRepository, PayPalHttpClient payPalHttpClient) : base(paymentRequestRepository, payPalHttpClient)
        {
        }

        public override Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input)
        {
            return Task.FromResult(new PaymentRequestStartResultDto
            {
                CheckoutLink = input.ReturnUrl + "?token=" + input.PaymentRequestId
            });
        }

        public override async Task<PaymentRequestDto> CompleteAsync(string token)
        {
            var paymentRequest = await PaymentRequestRepository.GetAsync(Guid.Parse(token));

            paymentRequest.SetAsCompleted();

            await PaymentRequestRepository.UpdateAsync(paymentRequest);

            return ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);
        }
    }
}
