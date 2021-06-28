using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    public class SaasServiceHttpApiHostMigrationsDbContext : AbpDbContext<SaasServiceHttpApiHostMigrationsDbContext>
    {
        public SaasServiceHttpApiHostMigrationsDbContext(DbContextOptions<SaasServiceHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSaasService();
        }
    }
}
