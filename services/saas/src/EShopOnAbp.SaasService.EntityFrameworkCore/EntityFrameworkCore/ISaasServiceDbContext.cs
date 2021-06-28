using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public interface ISaasServiceDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}