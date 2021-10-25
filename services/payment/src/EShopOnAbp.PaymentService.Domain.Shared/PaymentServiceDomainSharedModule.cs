using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using EShopOnAbp.PaymentService.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace EShopOnAbp.PaymentService
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class PaymentServiceDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<PaymentOptions>(configuration.GetSection("Payment"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PaymentServiceDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PaymentServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/PaymentService");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("PaymentService", typeof(PaymentServiceResource));
            });
        }
    }
}
