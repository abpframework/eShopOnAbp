using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.CmskitService.EntityFrameworkCore;

[ConnectionStringName(CmskitServiceDbProperties.ConnectionStringName)]
public interface ICmskitServiceDbContext : IEfCoreDbContext
{
}