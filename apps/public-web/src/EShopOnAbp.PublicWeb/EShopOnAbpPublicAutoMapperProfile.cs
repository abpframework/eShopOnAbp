using AutoMapper;
using EShopOnAbp.BasketService;
using EShopOnAbp.PaymentService;

namespace EShopOnAbp.PublicWeb
{
    public class EShopOnAbpPublicAutoMapperProfile : Profile
    {
        public EShopOnAbpPublicAutoMapperProfile()
        {
            CreateMap<BasketItemDto, PaymentRequestProductCreationDto>()
                .ForMember(p => p.ReferenceId, opts => opts.MapFrom(p => p.ProductId.ToString()))
                .ForMember(p => p.Name, opts => opts.MapFrom(p => p.ProductName))
                .ForMember(p => p.UnitPrice, opts => opts.MapFrom(p => p.TotalPrice / p.Count))
                .ForMember(p => p.Quantity, opts => opts.MapFrom(p => p.Count));
        }
    }
}
