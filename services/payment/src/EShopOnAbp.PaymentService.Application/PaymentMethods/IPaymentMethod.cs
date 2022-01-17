using System.Threading.Tasks;
using EShopOnAbp.PaymentService.PaymentRequests;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentMethods;

public interface IPaymentMethod : ITransientDependency
{
    int PaymentTypeId { get; }

    string PaymentType { get; }

    public Task<PaymentRequestStartResultDto> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartDto input);

    public Task<PaymentRequest> CompleteAsync(IPaymentRequestRepository paymentRequestRepository, string token);

    public Task HandleWebhookAsync(string payload);
}