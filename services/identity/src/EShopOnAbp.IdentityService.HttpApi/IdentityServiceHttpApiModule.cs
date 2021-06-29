using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace EShopOnAbp.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule)
    )]
    public class IdentityServiceHttpApiModule : AbpModule
    {
    }
}