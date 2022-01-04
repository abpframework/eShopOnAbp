using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Buyers;
using EShopOnAbp.OrderingService.Localization;
using Volo.Abp.Application.Services;
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
        return await CreateOrderDtoMappingAsync(order);
    }

    public async Task<OrderDto> CreateAsync(OrderCreateDto input)
    {
        var orderItems = GetProductListTuple(input.Products);

        var placedOrder = await _orderManager.CreateOrderAsync
        (
            paymentTypeId: input.PaymentTypeId,
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

        return await CreateOrderDtoMappingAsync(placedOrder);
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

    private async Task<OrderDto> CreateOrderDtoMappingAsync(Order placedOrder)
    {
        return new OrderDto()
        {
            Address = ObjectMapper.Map<Address, OrderAddressDto>(placedOrder.Address),
            Items = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(placedOrder.OrderItems),
            Buyer = ObjectMapper.Map<Buyer, BuyerDto>(placedOrder.Buyer),
            Id = placedOrder.Id,
            OrderDate = placedOrder.OrderDate,
            OrderStatus = placedOrder.OrderStatus.Name,
            OrderStatusId = placedOrder.OrderStatus.Id,
            PaymentType = placedOrder.PaymentType.Name,
            PaymentTypeId = placedOrder.PaymentType.Id
        };
    }
}