using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentRequests;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentServices;

public interface IPaymentMethod : ITransientDependency
{
    public int PaymentTypeId { get; }
    public Task<PaymentRequestStartResultDto> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartDto input);
    public Task<PaymentRequest> CompleteAsync(IPaymentRequestRepository paymentRequestRepository, string token);
}