using System;
using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PayPalCheckoutSdk.Core;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.PaymentService.PaymentServices;

public class PaymentServiceFactory : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentServiceFactory> _logger;

    public PaymentServiceFactory(IServiceProvider serviceProvider, ILogger<PaymentServiceFactory> logger)
    {
        logger.LogInformation("== Payment SErvice Factory == ");
        _serviceProvider = serviceProvider;
        _logger = logger;
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
            _logger.LogInformation($"=== Resolved PaypalService {paypalService} ===");
            var requestDomainService = _serviceProvider.GetRequiredService<PaymentRequestDomainService>();
            _logger.LogInformation($"=== Resolved PaymentRequestDomainService {requestDomainService} ===");

            return new PaypalService(paypalService, requestDomainService);
        }

        return new DemoService();
    }
}