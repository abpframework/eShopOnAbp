using System;

namespace EShopOnAbp.BasketService
{
    public interface IHasAnonymousId
    {
        Guid? AnonymousId { get; }
    }
}
