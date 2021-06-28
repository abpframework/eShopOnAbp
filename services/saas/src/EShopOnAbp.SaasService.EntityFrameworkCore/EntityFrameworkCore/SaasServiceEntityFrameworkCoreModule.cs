using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EShopOnAbp.SaasService.EntityFrameworkCore
{
    [DependsOn(
        typeof(SaasServiceDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class SaasServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SaasServiceDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}