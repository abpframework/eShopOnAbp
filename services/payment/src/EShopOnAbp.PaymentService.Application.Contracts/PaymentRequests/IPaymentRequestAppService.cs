using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.PaymentService.PaymentRequests
{
    public interface IPaymentRequestAppService : IApplicationService
    {
        Task<PaymentRequestDto> CreateAsync(PaymentRequestCreationDto input);

        Task<PaymentRequestStartResultDto> StartAsync(PaymentRequestStartDto input);

        Task<PaymentRequestDto> CompleteAsync(PaymentRequestCompleteInputDto input);

        Task<bool> HandleWebhookAsync(string payload);
        // /// <summary>
        // /// 0 - Demo
        // /// 1 - Paypal
        // /// </summary>
        // /// <param name="paymentTypeId"></param>
        // /// <returns></returns>
        // Task SetPaymentServiceAsync(int paymentTypeId);
    }
}
