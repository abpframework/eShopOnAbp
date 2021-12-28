using EShopOnAbp.OrderingService.Orders;
using EShopOnAbp.PaymentService.PaymentRequests;
using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace EShopOnAbp.OrderingService.PaymentRequests
{
    public class PaymentRequestEventHandler :
        IDistributedEventHandler<PaymentRequestCompletedEto>,
        ITransientDependency
    {
        private readonly IDistributedEventBus _eventBus;

        public PaymentRequestEventHandler(IDistributedEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task HandleEventAsync(PaymentRequestCompletedEto eventData)
        {
            // TODO: Insert Order here.
            //throw new NotImplementedException();

            await _eventBus.PublishAsync(new OrderAcceptedEto
            {
                Items = eventData.Products.Select(MapProductToOrderItem).ToList(),
                BuyerId = Guid.Parse(eventData.BuyerId),
                OrderId = Guid.Empty, // TODO: Inserted OrderId,
            });
        }

        private static OrderItemEto MapProductToOrderItem(PaymentRequestProductEto arg)
        {
            return new OrderItemEto
            {
                Count = arg.Quantity,
                ProductId = Guid.Parse(arg.ReferenceId)
            };
        }
    }
}
