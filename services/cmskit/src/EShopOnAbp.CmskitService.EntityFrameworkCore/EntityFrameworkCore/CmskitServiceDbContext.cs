using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

[ConnectionStringName(CmskitServiceDbProperties.ConnectionStringName)]
public class CmskitServiceDbContext : AbpDbContext<CmskitServiceDbContext>, ICmskitServiceDbContext
{
    public CmskitServiceDbContext(DbContextOptions<CmskitServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        FeatureConfigurer.Configure();

        modelBuilder.ConfigureCmskitService();
        modelBuilder.ConfigureCmsKit();
    }
}
