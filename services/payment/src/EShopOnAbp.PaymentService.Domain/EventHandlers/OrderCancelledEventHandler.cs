using EShopOnAbp.OrderingService.Orders;
using EShopOnAbp.PaymentService.PaymentRequests;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace EShopOnAbp.PaymentService.EventHandlers
{
    public class OrderCancelledEventHandler : IDistributedEventHandler<OrderCancelledEto>, ITransientDependency
    {
        private readonly IPaymentRequestRepository _paymentRepository;

        public OrderCancelledEventHandler(IPaymentRequestRepository paymenRepository)
        {
            _paymentRepository = paymenRepository;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(OrderCancelledEto eventData)
        {
            var payment = await _paymentRepository.GetAsync(eventData.Buyer.BuyerId.GetValueOrDefault());
            await _paymentRepository.DeleteAsync(payment);
        }
    }
}
