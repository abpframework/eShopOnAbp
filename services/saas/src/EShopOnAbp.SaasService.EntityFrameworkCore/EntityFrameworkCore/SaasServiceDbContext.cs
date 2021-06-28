using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public class SaasServiceDbContext : AbpDbContext<SaasServiceDbContext>, ISaasServiceDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public SaasServiceDbContext(DbContextOptions<SaasServiceDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSaasService();
        }
    }
}