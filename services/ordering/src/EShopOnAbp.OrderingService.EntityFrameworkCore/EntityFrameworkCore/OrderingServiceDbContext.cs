using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.OrderingService.EntityFrameworkCore
{
    [ConnectionStringName(OrderingServiceDbProperties.ConnectionStringName)]
    public class OrderingServiceDbContext :
        AbpDbContext<OrderingServiceDbContext>
    {
        public OrderingServiceDbContext(DbContextOptions<OrderingServiceDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigureOrderingService();
            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(OrderingServiceConsts.DbTablePrefix + "YourEntities", OrderingServiceConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}
