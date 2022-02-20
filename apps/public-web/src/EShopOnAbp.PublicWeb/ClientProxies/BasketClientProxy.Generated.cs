// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using EShopOnAbp.BasketService.Services;

// ReSharper disable once CheckNamespace
namespace EShopOnAbp.BasketService.Services.ClientProxies;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBasketAppService), typeof(BasketClientProxy))]
public partial class BasketClientProxy : ClientProxyBase<IBasketAppService>, IBasketAppService
{
    public virtual async Task<BasketDto> GetAsync(Guid? anonymousUserId)
    {
        return await RequestAsync<BasketDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid?), anonymousUserId }
        });
    }

    public virtual async Task<BasketDto> AddProductAsync(AddProductDto input)
    {
        return await RequestAsync<BasketDto>(nameof(AddProductAsync), new ClientProxyRequestTypeValue
        {
            { typeof(AddProductDto), input }
        });
    }

    public virtual async Task<BasketDto> RemoveProductAsync(RemoveProductDto input)
    {
        return await RequestAsync<BasketDto>(nameof(RemoveProductAsync), new ClientProxyRequestTypeValue
        {
            { typeof(RemoveProductDto), input }
        });
    }
}