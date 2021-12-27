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
                Name = "Home",
                Description = "Cecilia Chapman Senior 711-2880 Nulla St. Mankato Mississippi 96522/USA",
                IsDefault = true
            },
            new()
            {
                Name = "Work",
                Description = "Yeşilköy Serbest Bölge Mah. E-Blok Sokak E1 Blok No:2, Bakırköy/İstanbul"
            }
        };
    }
}

public class AddressDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; } = false;
}