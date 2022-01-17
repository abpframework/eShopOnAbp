using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public interface IPaymentRequestAppService : IApplicationService
    {
        Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input);

        Task<PaymentRequestStartResultDto> StartAsync(string paymentType, PaymentRequestStartDto input);

        Task<PaymentRequestDto> CompleteAsync(string paymentType, PaymentRequestCompleteInputDto input);

        Task<bool> HandleWebhookAsync(string paymentType, string payload);
    }
}
