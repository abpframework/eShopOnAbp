using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace EShopOnAbp.OrderingService.Orders;

public class Address : ValueObject
{
    public string Description { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    private Address()
    {
    }

    public Address(string street, string city, string description, string country, string zipcode)
    {
        Street = street;
        City = city;
        Description = description;
        Country = country;
        ZipCode = zipcode;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return Description;
        yield return Country;
        yield return ZipCode;
    }
}