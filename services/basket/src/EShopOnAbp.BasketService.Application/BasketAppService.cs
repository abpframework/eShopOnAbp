using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;
using EShopOnAbp.CatalogService.Products;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.EventBus.Distributed;

namespace EShopOnAbp.BasketService;

public class BasketAppService : BasketServiceAppService, IBasketAppService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBasketProductService _basketProductService;
    private readonly IDistributedEventBus _distributedEventBus;

    public BasketAppService(
        IBasketRepository basketRepository,
        IBasketProductService basketProductService,
        IDistributedEventBus distributedEventBus)
    {
        _basketRepository = basketRepository;
        _basketProductService = basketProductService;
        _distributedEventBus = distributedEventBus;
    }

    public async Task<BasketDto> GetAsync()
    {
        var basket = await _basketRepository.GetAsync(CurrentUser.GetId());
        return await GetBasketDtoAsync(basket);
    }

    public async Task<BasketDto> GetByAnonymousUserIdAsync(Guid id)
    {
        var basket = await _basketRepository.GetAsync(id);
        ;
        return await GetBasketDtoAsync(basket);
    }

    public async Task<BasketDto> MergeBasketsAsync()
    {
        //TODO: move to custom shared project
        var anonymousUserIdString = CurrentUser.FindClaimValue("anonymous_id");
        
        if (!Guid.TryParse(anonymousUserIdString, out Guid anonymousUserId))
        {
            Logger.LogError($"Couldn't parse anonymous Id from claim!{anonymousUserIdString}");
        }

        Basket anonymousUserBasket = await _basketRepository.GetAsync(anonymousUserId);
        if (!CurrentUser.IsAuthenticated)
        {
            Logger.LogWarning($"User is not authenticated! Merging baskets failed!");
            return await GetBasketDtoAsync(anonymousUserBasket);
        }

        var userBasket = await _basketRepository.GetAsync(CurrentUser.GetId());

        foreach (var item in anonymousUserBasket.Items)
        {
            userBasket.AddProduct(item.ProductId, item.Count);
        }

        await _basketRepository.UpdateAsync(userBasket);
        anonymousUserBasket.Clear();
        await _basketRepository.UpdateAsync(anonymousUserBasket);

        return await GetBasketDtoAsync(userBasket);
    }

    public async Task<BasketDto> AddProductAsync(AddProductDto input)
    {
        Guid userId = CurrentUser.IsAuthenticated ? CurrentUser.GetId() : input.AnonymousId.GetValueOrDefault();

        var basket = await _basketRepository.GetAsync(userId);
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
        Guid userId = CurrentUser.IsAuthenticated ? CurrentUser.GetId() : input.AnonymousId.GetValueOrDefault();

        var basket = await _basketRepository.GetAsync(userId);
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