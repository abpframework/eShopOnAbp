using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;
using EShopOnAbp.CatalogService.Products;

namespace EShopOnAbp.BasketService;

[Authorize]
public class BasketAppService : BasketServiceAppService, IBasketAppService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketProductService _basketProductService;

    public BasketAppService(
        IBasketRepository basketRepository,
        IBasketProductService basketProductService)
    {
        _basketRepository = basketRepository;
        _basketProductService = basketProductService;
    }
    
    public async Task<BasketDto> GetAsync()
    {
        var basket = await _basketRepository.GetAsync(CurrentUser.GetId());
        return await GetBasketDtoAsync(basket);
    }

    public async Task<BasketDto> AddProductAsync(AddProductDto input)
    {
        var basket = await _basketRepository.GetAsync(CurrentUser.GetId());
        var product = await _basketProductService.GetAsync(input.ProductId);
        
        basket.AddProduct(product.Id);
        
        return await GetBasketDtoAsync(basket);
    }

    public async Task<BasketDto> RemoveProductAsync(RemoveProductDto input)
    {
        var basket = await _basketRepository.GetAsync(CurrentUser.GetId());
        var product = await _basketProductService.GetAsync(input.ProductId);
        
        basket.RemoveProduct(product.Id, input.Count);
        
        return await GetBasketDtoAsync(basket);
    }
    
    private async Task<BasketDto> GetBasketDtoAsync(Basket basket)
    {
        var products = new Dictionary<Guid, ProductDto>();

        var basketDto = new BasketDto();

        foreach (var basketItem in basket.Items)
        {
            var basketItemDto = new BasketItemDto
            {
                ProductId = basketItem.ProductId,
                Count = basketItem.Count
            };

            var productDto = products.GetOrDefault(basketItem.ProductId);
            if (productDto == null)
            {
                productDto = await _basketProductService.GetAsync(basketItem.ProductId);
                products[productDto.Id] = productDto;
            }

            basketItemDto.ProductCode = productDto.Code;
            basketItemDto.ImageName = productDto.ImageName;
            basketItemDto.ProductName = productDto.Name;
            basketItemDto.TotalPrice = productDto.Price * basketItemDto.Count;
        }

        basketDto.TotalPrice = basketDto.Items.Sum(x => x.TotalPrice);

        return basketDto;
    }
}