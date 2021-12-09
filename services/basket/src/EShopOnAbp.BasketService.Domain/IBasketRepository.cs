using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EShopOnAbp.BasketService;

public interface IBasketRepository : IRepository
{
    Task<Basket> GetAsync(Guid id);

    Task UpdateAsync(Basket basket);
}