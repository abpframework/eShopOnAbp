using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Microsoft.Extensions.Options;
using EShopOnAbp.PaymentService.PayPal;
using PayPalCheckoutSdk.Core;
using System;

namespace EShopOnAbp.PaymentService
{
    [DependsOn(
        typeof(PaymentServiceDomainModule),
        typeof(PaymentServiceApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class PaymentServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PaymentServiceApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PaymentServiceApplicationModule>(validate: true);
            });

            context.Services.AddTransient(provider =>
            {
                var options = provider.GetService<IOptions<PayPalOptions>>().Value;

                if (options.Environment.IsNullOrWhiteSpace() || options.Environment == PayPalConsts.Environment.Sandbox)
                {
                    return new PayPalHttpClient(new SandboxEnvironment(options.ClientId, options.Secret));
                }

                return new PayPalHttpClient(new LiveEnvironment(options.ClientId, options.Secret));
            });
        }
    }
}
