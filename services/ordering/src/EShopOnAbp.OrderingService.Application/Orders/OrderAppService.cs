using EShopOnAbp.OrderingService.Localization;
using EShopOnAbp.OrderingService.Orders.Specifications;
using EShopOnAbp.OrderingService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;
using Volo.Abp.Users;

namespace EShopOnAbp.OrderingService.Orders;

public class OrderAppService : ApplicationService, IOrderAppService
{
    private readonly OrderManager _orderManager;
    private readonly IOrderRepository _orderRepository;

    public OrderAppService(OrderManager orderManager,
        IOrderRepository orderRepository
    )
    {
        _orderManager = orderManager;
        _orderRepository = orderRepository;
        LocalizationResource = typeof(OrderingServiceResource);
        ObjectMapperContext = typeof(OrderingServiceApplicationModule);
    }

    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        return CreateOrderDtoMapping(order);
    }

    public async Task<List<OrderDto>> GetMyOrdersAsync(GetMyOrdersInput input)
    {
        ISpecification<Order> specification = SpecificationFactory.Create(input.Filter);
        var orders = await _orderRepository.GetOrdersByUserId(CurrentUser.GetId(), specification, true);
        return CreateOrderDtoMapping(orders);
    }

    [Authorize(OrderingServicePermissions.Orders.Default)]
    public async Task<List<OrderDto>> GetOrdersAsync(GetOrdersInput input)
    {
        ISpecification<Order> specification = SpecificationFactory.Create(input.Filter);
        var orders = await _orderRepository.GetOrders(specification, true);
        return CreateOrderDtoMapping(orders);
    }

    public async Task<OrderDto> GetByOrderNoAsync(int orderNo)
    {
        var order = await _orderRepository.GetByOrderNoAsync(orderNo);
        Logger.LogInformation($" Order recieved with order no:{orderNo}");
        return CreateOrderDtoMapping(order);
    }

    public async Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input)
    {
        var order = await _orderRepository.GetAsync(id);
        if (input.OrderStatusId == OrderStatus.Shipped.Id)
        {
            order.SetOrderAsShipped(input.OrderStatusId);
            await _orderRepository.UpdateAsync(order);
        }
        else if (input.OrderStatusId == OrderStatus.Cancelled.Id)
        {
            await _orderManager.CancelOrderAsync(id, input.PaymentRequestId, input.PaymentRequestStatus);
        }

        return CreateOrderDtoMapping(order);
    }

    public async Task<OrderDto> CreateAsync(OrderCreateDto input)
    {
        var orderItems = GetProductListTuple(input.Products);

        var placedOrder = await _orderManager.CreateOrderAsync
        (
            paymentMethod: input.PaymentMethod,
            buyerId: CurrentUser.GetId(),
            buyerName: CurrentUser.Name,
            buyerEmail: CurrentUser.Email,
            orderItems: orderItems,
            addressStreet: input.Address.Street,
            addressCity: input.Address.City,
            addressCountry: input.Address.Country,
            addressZipCode: input.Address.ZipCode,
            addressDescription: input.Address.Description
        );

        return CreateOrderDtoMapping(placedOrder);
    }

    private List<(Guid productId, string productName, string productCode, decimal unitPrice, decimal discount, string
        pictureUrl, int units
        )> GetProductListTuple(List<OrderItemCreateDto> products)
    {
        var orderItems =
            new List<(Guid productId, string productName, string productCode, decimal unitPrice, decimal discount,
                string pictureUrl, int
                units)>();
        foreach (var product in products)
        {
            orderItems.Add((product.ProductId, product.ProductName, product.ProductCode, product.UnitPrice,
                product.Discount, product.PictureUrl, product.Units));
        }

        return orderItems;
    }

    private List<OrderDto> CreateOrderDtoMapping(List<Order> orders)
    {
        List<OrderDto> dtoList = new List<OrderDto>();
        foreach (var order in orders)
        {
            dtoList.Add(CreateOrderDtoMapping(order));
        }

        return dtoList;
    }

    private OrderDto CreateOrderDtoMapping(Order order)
    {
        return new OrderDto()
        {
            Address = ObjectMapper.Map<Address, OrderAddressDto>(order.Address),
            Items = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(order.OrderItems),
            Buyer = ObjectMapper.Map<Buyer, BuyerDto>(order.Buyer),
            Id = order.Id,
            OrderNo = order.OrderNo,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus.Name,
            OrderStatusId = order.OrderStatus.Id,
            PaymentMethod = order.PaymentMethod
        };
    }
}