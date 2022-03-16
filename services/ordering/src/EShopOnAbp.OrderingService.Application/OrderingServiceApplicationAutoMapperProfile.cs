using AutoMapper;
using EShopOnAbp.OrderingService.Orders;
using EShopOnAbp.OrderingService.OrderItems;
using Volo.Abp.AutoMapper;

namespace EShopOnAbp.OrderingService
{
    public class OrderingServiceApplicationAutoMapperProfile : Profile
    {
        public OrderingServiceApplicationAutoMapperProfile()
        {
            CreateMap<Address, OrderAddressDto>();
            CreateMap<Buyer, BuyerDto>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItem, TopSellingDto>();
            CreateMap<Order, PaymentDto>();

            CreateMap<Order, OrderDto>()
                .Ignore(q => q.Address)
                .Ignore(q => q.Items)
                .Ignore(q => q.Buyer);
        }
    }
}