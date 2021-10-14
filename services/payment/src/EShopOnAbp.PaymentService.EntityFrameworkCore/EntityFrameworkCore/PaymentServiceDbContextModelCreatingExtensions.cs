using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace EShopOnAbp.PaymentService.EntityFrameworkCore
{
    public static class PaymentServiceDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentService(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(PaymentServiceDbProperties.DbTablePrefix + "Questions", PaymentServiceDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}
