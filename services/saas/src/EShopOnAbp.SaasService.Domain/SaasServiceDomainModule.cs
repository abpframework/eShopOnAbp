using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(AbpTenantManagementDomainModule),
        typeof(SaasServiceDomainSharedModule)
    )]
    public class SaasServiceDomainModule : AbpModule
    {

    }
}
