using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace EShopOnAbp.OrderingService.Buyers;

public class Buyer : AggregateRoot<Guid>
{
    public string UserName { get; private set; }
    public string Name { get; private set; }
    public string PaymentId { get; private set; }

    private Buyer()
    {
    }

    public Buyer(Guid id, [NotNull] string userName, [NotNull] string name, [NotNull] string paymentId) : base(id)
    {
        UserName = Check.NotNullOrEmpty(userName, nameof(userName));
        Name = Check.NotNullOrEmpty(name, nameof(name));
        PaymentId = Check.NotNullOrEmpty(paymentId, nameof(paymentId));
    }
}