using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace EShopOnAbp.OrderingService.Orders;

public class Buyer: ValueObject
{
    public Guid? Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    private Buyer()
    {
        
    }

    public Buyer(string email, string name, Guid? id)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        Email = Check.NotNullOrEmpty(email, nameof(email));
        Id = id;
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Email;
    }
}