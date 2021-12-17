using AutoMapper;
using EShopOnAbp.CatalogService.Products;

namespace EShopOnAbp.BasketService
{
    public class BasketServiceApplicationAutoMapperProfile : Profile
    {
        public BasketServiceApplicationAutoMapperProfile()
        {
            CreateMap<ProductEto, ProductDto>();
            CreateMap<Basket, OrderAcceptedEto>();
            CreateMap<BasketItem, OrderItemEto>();
        }
    }
}
