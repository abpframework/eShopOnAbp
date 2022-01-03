using EShopOnAbp.OrderingService.Orders;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace EShopOnAbp.BasketService.Orders
{
    public class OrderPlacedEventHandler : IDistributedEventHandler<OrderPlacedEto>, ITransientDependency
    {
        private readonly IBasketRepository _basketRepository;

        public OrderPlacedEventHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task HandleEventAsync(OrderPlacedEto eventData)
        {
            var basket = await _basketRepository.GetAsync(eventData.Buyer.BuyerId);
            basket.Clear();
            await _basketRepository.UpdateAsync(basket);
        }
    }
}