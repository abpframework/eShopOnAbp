using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Buyers;
using Volo.Abp.Domain.Services;

namespace EShopOnAbp.OrderingService.Orders;

public class OrderManager : DomainService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;

    public OrderManager(IOrderRepository orderRepository, IBuyerRepository buyerRepository)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
    }

    public async Task<Order> CreateOrderAsync(
        int paymentTypeId,
        string buyerName,
        string buyerEmail,
        List<(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units)>
            orderItems,
        string addressStreet,
        string addressCity,
        string addressCountry,
        string addressZipCode,
        string addressDescription = null
    )
    {
        // Create buyer 
        Buyer buyer = await _buyerRepository.InsertAsync(
            new Buyer(GuidGenerator.Create(), buyerName, buyerEmail, PaymentType.From(paymentTypeId)),
            autoSave: true
        );

        // Create new order
        Order order = new Order(
            id: GuidGenerator.Create(),
            address: new Address(street: addressStreet,
                city: addressCity,
                country: addressCountry,
                zipcode: addressZipCode,
                description: addressDescription),
            buyerId: buyer.Id
        );

        // Add new order items
        foreach (var orderItem in orderItems)
        {
            order.AddOrderItem(
                productId: orderItem.productId,
                productName: orderItem.productName,
                unitPrice: orderItem.unitPrice,
                discount: orderItem.discount,
                pictureUrl: orderItem.pictureUrl,
                units: orderItem.units
            );
        }

        return await _orderRepository.InsertAsync(order);
    }
}