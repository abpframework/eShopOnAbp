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
    public class OrderingServiceDbContext : AbpDbContext<OrderingServiceDbContext>, IOrderingServiceDbContext
    {
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
            

            builder.Entity<Order>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "Orders", OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                
                b.Property(q => q.PaymentStatus).HasMaxLength(OrderConstants.PaymentStatusMaxLength);
                
                b.OwnsOne(o => o.Address, a => { a.WithOwner(); });
                b.OwnsOne(o => o.Buyer, a => { a.WithOwner(); });
                
                b.Property<int>("_orderStatusId").UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName("OrderStatusId")
                    .IsRequired();
                
                b.Property<int>("_paymentTypeId").UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName("PaymentTypeId")
                    .IsRequired();

                b.HasOne(q => q.OrderStatus).WithMany().HasForeignKey("_orderStatusId");
                b.HasOne(q => q.PaymentType).WithMany().HasForeignKey("_paymentTypeId");

                b.Navigation(q => q.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Property);
            });

            builder.Entity<OrderItem>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "OrderItems",
                    OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                b.Property<Guid>("OrderId").IsRequired();
                b.Property(q => q.ProductId).IsRequired();
                b.Property(q => q.ProductCode).IsRequired();
                b.Property(q => q.ProductName).IsRequired();
                b.Property(q => q.Discount).IsRequired();
                b.Property(q => q.UnitPrice).IsRequired();
                b.Property(q => q.Units).IsRequired();
                b.Property(q => q.PictureUrl).IsRequired(false);
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
            
            builder.Entity<PaymentType>(b =>
            {
                b.ToTable(OrderingServiceDbProperties.DbTablePrefix + "PaymentTypes",
                    OrderingServiceDbProperties.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                b.HasKey(q => q.Id);
                
                b.Property(q => q.Id)
                    .HasDefaultValue(1)
                    .ValueGeneratedNever()
                    .IsRequired();
                
                b.Property(o => o.Name)
                    .HasMaxLength(OrderConstants.OrderPaymentTypeNameMaxLength)
                    .IsRequired();
            });
        }
    }
}