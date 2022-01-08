using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace EShopOnAbp.OrderingService.Orders;

public class PaymentType : Enumeration
{
    public static PaymentType Paypal = new PaymentType(1, nameof(Paypal).ToLowerInvariant());
    public static PaymentType Demo = new PaymentType(2, nameof(Paypal).ToLowerInvariant());

    public PaymentType(int id, string name) : base(id, name)
    {
    }

    public static IEnumerable<PaymentType> List() =>
        new[] {Paypal, Demo};
    
    public static PaymentType FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new BusinessException(OrderingServiceErrorCodes.PaymentTypeNotFound)
                .WithData("PaymentType", String.Join(",", List().Select(s => s.Name)));
        }

        return state;
    }

    public static PaymentType From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        if (state == null)
        {
            throw new BusinessException(OrderingServiceErrorCodes.PaymentTypeNotFound)
                .WithData("PaymentType", String.Join(",", List().Select(s => s.Name)));
        }

        return state;
    }
}