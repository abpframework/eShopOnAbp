using System;
using EShopOnAbp.OrderingService.Buyers;
using EShopOnAbp.OrderingService.Orders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EShopOnAbp.OrderingService.EntityFrameworkCore
{
    [ConnectionStringName(OrderingServiceDbProperties.ConnectionStringName)]
    public class OrderingServiceDbContext :
        AbpDbContext<OrderingServiceDbContext>
    {
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

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

            builder.Entity<Buyer>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "Buyers", OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                b.Property(q => q.UserName).IsRequired();
                b.Property(q => q.Name).IsRequired();
                b.Property(q => q.PaymentId).IsRequired();
            });

            builder.Entity<Order>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "Orders", OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.OwnsOne(o => o.Address, a =>
                {
                    // Explicit configuration of the shadow key property in the owned type 
                    // as a workaround for a documented issue in EF Core 5: https://github.com/dotnet/efcore/issues/20740
                    a.Property<Guid>("OrderId")
                        .UseHiLo("orderseq", OrderingServiceDbProperties.DbSchema);
                    a.WithOwner();
                });
                b.Property<int>("_orderStatusId").UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName("OrderStatusId")
                    .IsRequired();
                b.Property(q => q.Description).HasMaxLength(OrderConstants.OrderDescriptionMaxLength).IsRequired(false);

                b.HasOne<Buyer>().WithMany().HasForeignKey(q => q.BuyerId).IsRequired(false);
                b.HasOne(q => q.OrderStatus).WithMany().HasForeignKey("_orderStatusId");

                b.Navigation(q => q.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Property);

                b.HasIndex(q => q.Id);
                b.HasIndex(q => q.BuyerId);
            });

            builder.Entity<OrderStatus>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "OrderStatus",
                    OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                b.HasKey(q => q.Id);
                b.Property(q => q.Id)
                    .HasDefaultValue(1)
                    .ValueGeneratedNever()
                    .IsRequired();
                b.Property(o => o.Name)
                    .HasMaxLength(OrderConstants.OrderStatusNameMaxLength)
                    .IsRequired();
            });

            builder.Entity<OrderItem>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "OrderItems",
                    OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                b.Property<Guid>("OrderId").IsRequired();
                b.Property(q=>q.ProductId).IsRequired();
                b.Property(q=>q.ProductName).IsRequired();
                b.Property(q=>q.Discount).IsRequired();
                b.Property(q=>q.UnitPrice).IsRequired();
                b.Property(q=>q.Units).IsRequired();
                b.Property(q=>q.PictureUrl).IsRequired(false);
            });
        }
    }
}