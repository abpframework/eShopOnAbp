using System;
using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentRequests;

namespace EShopOnAbp.PaymentService.PaymentServices;

public class DemoService : IPaymentStrategy
{
    public Task<PaymentRequestStartResultDto> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartDto input)
    {
        return Task.FromResult(new PaymentRequestStartResultDto
        {
            CheckoutLink = input.ReturnUrl + "?token=" + input.PaymentRequestId
        });
    }

    public async Task<PaymentRequest> CompleteAsync(IPaymentRequestRepository paymentRequestRepository, string token)
    {
        var paymentRequest = await paymentRequestRepository.GetAsync(Guid.Parse(token));

        paymentRequest.SetAsCompleted();

        return await paymentRequestRepository.UpdateAsync(paymentRequest);
    }
}