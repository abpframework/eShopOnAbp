using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.CatalogService.EntityFrameworkCore
{
    [ConnectionStringName(CatalogServiceDbProperties.ConnectionStringName)]
    public class CatalogServiceDbContext :
        AbpDbContext<CatalogServiceDbContext>
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */

        public CatalogServiceDbContext(DbContextOptions<CatalogServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(CatalogServiceConsts.DbTablePrefix + "YourEntities", CatalogServiceConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}
