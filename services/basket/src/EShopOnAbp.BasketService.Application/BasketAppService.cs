using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;
using EShopOnAbp.CatalogService.Products;
using Volo.Abp;

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

        if (basket.GetProductCount(product.Id) >= product.StockCount)
        {
            throw new UserFriendlyException("There is not enough product in stock, sorry :(");
        }
        
        basket.AddProduct(product.Id);

        await _basketRepository.UpdateAsync(basket);
        
        return await GetBasketDtoAsync(basket);
    }

    public async Task<BasketDto> RemoveProductAsync(RemoveProductDto input)
    {
        var basket = await _basketRepository.GetAsync(CurrentUser.GetId());
        var product = await _basketProductService.GetAsync(input.ProductId);
        
        basket.RemoveProduct(product.Id, input.Count);
        
        await _basketRepository.UpdateAsync(basket);
        
        return await GetBasketDtoAsync(basket);
    }
    
    private async Task<BasketDto> GetBasketDtoAsync(Basket basket)
    {
        var products = new Dictionary<Guid, ProductDto>();
        var basketDto = new BasketDto();
        var basketChanged = false;

        foreach (var basketItem in basket.Items)
        {
            var productDto = products.GetOrDefault(basketItem.ProductId);
            if (productDto == null)
            {
                productDto = await _basketProductService.GetAsync(basketItem.ProductId);
                products[productDto.Id] = productDto;
            }
            
            //Removing the products if not available in the stock
            if (basketItem.Count > productDto.StockCount)
            {
                basket.RemoveProduct(basketItem.ProductId, basketItem.Count - productDto.StockCount);
                basketChanged = true;
            }

            basketDto.Items.Add(new BasketItemDto
            {
                ProductId = basketItem.ProductId,
                Count = basketItem.Count,
                ProductCode = productDto.Code,
                ImageName = productDto.ImageName,
                ProductName = productDto.Name,
                TotalPrice = productDto.Price * basketItem.Count
            });
        }

        basketDto.TotalPrice = basketDto.Items.Sum(x => x.TotalPrice);

        if (basketChanged)
        {
            await _basketRepository.UpdateAsync(basket);
        }
        
        return basketDto;
    }
}