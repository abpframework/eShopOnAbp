using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.SaasService.MongoDB
{
    [DependsOn(
        typeof(SaasServiceDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class SaasServiceMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<SaasServiceMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });
        }
    }
}
