using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Buyers;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace EShopOnAbp.OrderingService.Orders;

public class OrderManager : DomainService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly UnitOfWorkManager _unitOfWorkManager;

    public OrderManager(
        IOrderRepository orderRepository,
        IBuyerRepository buyerRepository,
        IDistributedEventBus distributedEventBus, UnitOfWorkManager unitOfWorkManager)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
        _distributedEventBus = distributedEventBus;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task<Order> CreateOrderAsync(
        int paymentTypeId,
        string buyerName,
        string buyerEmail,
        List<(Guid productId, string productName, string productCode, decimal unitPrice, decimal discount, string
                pictureUrl, int units)>
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

        var insertedBuyer = await _buyerRepository.GetAsync(buyer.Id, includeDetails: true);

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
                id: GuidGenerator.Create(),
                productId: orderItem.productId,
                productName: orderItem.productName,
                productCode: orderItem.productCode,
                unitPrice: orderItem.unitPrice,
                discount: orderItem.discount,
                pictureUrl: orderItem.pictureUrl,
                units: orderItem.units
            );
        }

        var placedOrder = await _orderRepository.InsertAsync(order, true);

        // Publish Order placed event
        await _distributedEventBus.PublishAsync(new OrderPlacedEto
        {
            OrderId = placedOrder.Id,
            OrderDate = placedOrder.OrderDate,
            Buyer = GetBuyerEto(buyer),
            Items = GetProductItemEtoList(order.OrderItems)
        });

        return placedOrder;
    }

    public async Task<Order> AcceptOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);
        if (order == null)
        {
            throw new BusinessException(OrderingServiceErrorCodes.OrderWithIdNotFound)
                .WithData("OrderId", orderId);
        }

        //Update order.PaymentId
        //Update order.PaymentStatus
        order.SetOrderPaid();

        return await _orderRepository.UpdateAsync(order);
    }

    private BuyerEto GetBuyerEto(Buyer buyer)
    {
        return new BuyerEto()
        {
            BuyerEmail = buyer.Email,
            BuyerId = buyer.Id,
            PaymentType = buyer.PaymentType.Name,
            PaymentTypeId = buyer.PaymentType.Id
        };
    }

    private List<OrderItemEto> GetProductItemEtoList(List<OrderItem> orderItems)
    {
        List<OrderItemEto> etoList = new List<OrderItemEto>();
        foreach (var oItem in orderItems)
        {
            etoList.Add(new OrderItemEto()
            {
                Discount = oItem.Discount,
                PictureUrl = oItem.PictureUrl,
                ProductCode = oItem.ProductCode,
                ProductId = oItem.ProductId,
                ProductName = oItem.ProductName,
                UnitPrice = oItem.UnitPrice,
                Units = oItem.Units
            });
        }

        return etoList;
    }
}