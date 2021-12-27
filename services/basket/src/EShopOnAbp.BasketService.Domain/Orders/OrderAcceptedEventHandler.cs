using EShopOnAbp.OrderingService.Orders;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace EShopOnAbp.BasketService.Orders
{
    public class OrderAcceptedEventHandler :
        IDistributedEventHandler<OrderAcceptedEto>,
        ITransientDependency
    {
        private readonly IBasketRepository _basketRepository;

        public OrderAcceptedEventHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task HandleEventAsync(OrderAcceptedEto eventData)
        {
            var basket = await _basketRepository.GetAsync(eventData.BuyerId);
            basket.Clear();
            await _basketRepository.UpdateAsync(basket);
        }
    }
}
