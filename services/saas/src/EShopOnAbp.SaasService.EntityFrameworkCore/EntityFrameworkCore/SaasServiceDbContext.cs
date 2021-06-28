using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public class SaasServiceDbContext : AbpDbContext<SaasServiceDbContext>, ITenantManagementDbContext
    {
        public DbSet<Tenant> Tenants { get; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; }

        public SaasServiceDbContext(DbContextOptions<SaasServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ConfigureTenantManagement();
        }
    }
}