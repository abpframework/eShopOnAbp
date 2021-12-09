using System;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.Products;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.BasketService;

public class BasketProductService : IBasketProductService, ITransientDependency
{
    private readonly IPublicProductAppService _productAppService;
    private readonly IDistributedCache<ProductDto, Guid> _cache;

    public BasketProductService(
        IPublicProductAppService productAppService,
        IDistributedCache<ProductDto, Guid> cache)
    {
        _productAppService = productAppService;
        _cache = cache;
    }
    
    public async Task<ProductDto> GetAsync(Guid productId)
    {
        return await _cache.GetOrAddAsync(
            productId,
            () => GetProductAsync(productId)
        );
    }

    private Task<ProductDto> GetProductAsync(Guid productId)
    {
        return _productAppService.GetAsync(productId) ?? 
               throw new UserFriendlyException("Could not find the product!"); //TODO: Business exception with localization;
    }
}