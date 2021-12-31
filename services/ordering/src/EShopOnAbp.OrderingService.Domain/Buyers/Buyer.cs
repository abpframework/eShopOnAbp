using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace EShopOnAbp.OrderingService.Buyers;

public class Buyer : AggregateRoot<Guid>
{
    private int _paymentTypeId;
    public string Name { get; private set; }
    public string Email { get; private set; }
    public PaymentType PaymentType { get; private set; }

    private Buyer()
    {
    }

    internal Buyer(Guid id,
        [NotNull] string name,
        [NotNull] string email,
        PaymentType paymentType) : base(id)
    {
        _paymentTypeId = PaymentType.FromName(paymentType.Name).Id;
        Name = Check.NotNullOrEmpty(name, nameof(name));
        Email = Check.NotNullOrEmpty(email, nameof(email));
    }
}