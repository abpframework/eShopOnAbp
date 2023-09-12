using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

public static class CmskitServiceDbContextModelCreatingExtensions
{
    public static void ConfigureCmskitService(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(CmskitServiceConsts.DbTablePrefix + "YourEntities", CmskitServiceConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
