using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public interface IPaymentRequestAppService : IApplicationService
    {
        Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input);

        Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input);

        Task<PaymentRequestDto> CompleteAsync(string token);

        Task<bool> HandleWebhookAsync(string payload);
    }
}
