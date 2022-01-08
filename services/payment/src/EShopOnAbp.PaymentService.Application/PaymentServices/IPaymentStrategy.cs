using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentRequests;

namespace EShopOnAbp.PaymentService.PaymentServices;

public interface IPaymentStrategy
{
    public Task<PaymentRequestStartResultDto> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartDto input);
    public Task<PaymentRequest> CompleteAsync(IPaymentRequestRepository paymentRequestRepository, string token);
}