namespace EShopOnAbp.BasketService.Services.Dtos
{
    public interface IHasAnonymousId
    {
        Guid? AnonymousId { get; }
    }
}
