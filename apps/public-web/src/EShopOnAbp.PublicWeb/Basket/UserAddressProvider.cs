using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PublicWeb.Basket;

public class UserAddressProvider : ITransientDependency
{
    public List<AddressDto> GetDemoAddresses()
    {
        return new List<AddressDto>()
        {
            new()
            {
                Id = 1,
                Name = EShopOnAbpPaymentConsts.DemoAddressTypes.Home,
                Description = "Cecilia Chapman Senior 711-2880 Nulla St. Mankato Mississippi 96522/USA",
                IsDefault = true
            },
            new()
            {
                Id = 2,
                Name = EShopOnAbpPaymentConsts.DemoAddressTypes.Work,
                Description = "Yeşilköy Serbest Bölge Mah. E-Blok Sokak E1 Blok No:2, Bakırköy/İstanbul"
            }
        };
    }
}

public class AddressDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; } = false;
}