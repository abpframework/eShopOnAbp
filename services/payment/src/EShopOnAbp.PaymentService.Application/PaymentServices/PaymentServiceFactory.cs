using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentServices;

public class PaymentServiceFactory : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentStrategy Create(int paymentTypeId)
    {
        if (paymentTypeId == 0)
        {
            return _serviceProvider.GetRequiredService<DemoService>();
        }

        if (paymentTypeId == 1)
        {
            return _serviceProvider.GetRequiredService<PaypalService>();
        }

        return _serviceProvider.GetRequiredService<DemoService>();
    }
}