using System;
using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.Extensions.DependencyInjection;
using PayPalCheckoutSdk.Core;
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
            return new DemoService();
        }

        if (paymentTypeId == 1)
        {
            var paypalService = _serviceProvider.GetRequiredService<PayPalHttpClient>();
            var requestDomainService = _serviceProvider.GetRequiredService<PaymentRequestDomainService>();

            return new PaypalService(paypalService, requestDomainService);
        }

        return new DemoService();
    }
}