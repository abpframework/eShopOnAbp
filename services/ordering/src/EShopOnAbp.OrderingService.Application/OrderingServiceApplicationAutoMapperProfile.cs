using AutoMapper;
using EShopOnAbp.OrderingService.Buyers;
using EShopOnAbp.OrderingService.Orders;
using Volo.Abp.AutoMapper;

namespace EShopOnAbp.OrderingService
{
    public class OrderingServiceApplicationAutoMapperProfile : Profile
    {
        public OrderingServiceApplicationAutoMapperProfile()
        {
            CreateMap<Address, OrderAddressDto>();
            
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Buyer, BuyerDto>()
                .ForMember(q => q.PaymentType, opt => opt.MapFrom(q => q.PaymentType.Name))
                .ForMember(q => q.PaymentTypeId, opt => opt.MapFrom(q => q.PaymentType.Id));

            CreateMap<Order, OrderDto>()
                .Ignore(q => q.Address)
                .Ignore(q => q.Items)
                .Ignore(q => q.Buyer);
        }
    }
}