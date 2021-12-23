using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.BasketService;

public interface IBasketAppService : IApplicationService
{
    Task<BasketDto> GetAsync();
    Task<BasketDto> GetByAnonymousUserIdAsync(Guid id);
    Task<BasketDto> AddProductAsync(AddProductDto input);
    Task<BasketDto> RemoveProductAsync(RemoveProductDto input);
    Task PurchaseAsync();
}