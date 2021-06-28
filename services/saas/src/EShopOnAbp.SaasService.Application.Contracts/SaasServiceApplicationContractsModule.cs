using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(SaasServiceDomainSharedModule),
        typeof(AbpTenantManagementApplicationContractsModule)
        )]
    public class SaasServiceApplicationContractsModule : AbpModule
    {

    }
}
