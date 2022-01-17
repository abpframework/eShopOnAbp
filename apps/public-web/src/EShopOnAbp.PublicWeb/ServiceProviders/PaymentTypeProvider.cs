using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PublicWeb.ServiceProviders;

public class PaymentTypeProvider : ITransientDependency
{
    public List<PaymentType> GetPaymentTypes()
    {
        return new List<PaymentType>
        {
            new() { Type = "demo", Name = "Demo", IconCss = "fa-credit-card demo", IsDefault = true},
            new() { Type = "paypal", Name = "Paypal", IconCss = "fa-cc-paypal paypal"}
        };
    }
}

public class PaymentType
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string IconCss { get; set; }
    public bool IsDefault { get; set; } = false;
}