using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EShopOnAbp.PaymentService.EntityFrameworkCore
{
    public static class PaymentServiceDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentService(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<PaymentRequest>(entity =>
            {
                entity.ConfigureByConvention();

                entity
                    .Property(p => p.Currency)
                    .IsRequired()
                    .HasMaxLength(PaymentRequestConsts.MaxCurrencyLength);
            });

        }
    }
}
