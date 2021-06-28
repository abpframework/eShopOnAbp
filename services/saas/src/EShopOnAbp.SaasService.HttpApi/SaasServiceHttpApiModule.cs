using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceApplicationContractsModule),
        typeof(AbpTenantManagementHttpApiModule)
        )]
    public class SaasServiceHttpApiModule : AbpModule
    {
    }
}
