using System;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.Grpc;
using EShopOnAbp.CatalogService.Products;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Caching;

namespace EShopOnAbp.BasketService;

public class BasketProductService : BasketServiceAppService, IBasketProductService
{
    private readonly IDistributedCache<ProductDto, Guid> _cache;
    private readonly ProductPublic.ProductPublicClient _productPublicGrpcClient;

    public BasketProductService(
        IDistributedCache<ProductDto, Guid> cache,
        ProductPublic.ProductPublicClient productPublicGrpcClient
    )
    {
        _cache = cache;
        _productPublicGrpcClient = productPublicGrpcClient;
    }

    public async Task<ProductDto> GetAsync(Guid productId)
    {
        return await _cache.GetOrAddAsync(
            productId,
            () => GetProductAsync(productId)
        );
    }

    private async Task<ProductDto> GetProductAsync(Guid productId)
    {
        var request = new ProductRequest { Id = productId.ToString() };
        Logger.LogInformation("=== GRPC request {@request}", request);
        var response = await _productPublicGrpcClient.GetByIdAsync(request);
        Logger.LogInformation("=== GRPC response {@response}", response);
        return ObjectMapper.Map<ProductResponse, ProductDto>(response) ??
               throw new UserFriendlyException(BasketServiceDomainErrorCodes.ProductNotFound);
    }
}