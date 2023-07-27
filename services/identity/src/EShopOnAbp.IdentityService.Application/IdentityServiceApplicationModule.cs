using EShopOnAbp.IdentityService.Keycloak;
using EShopOnAbp.IdentityService.Keycloak.Service;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace EShopOnAbp.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceDomainModule),
        typeof(IdentityServiceApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpBackgroundJobsModule)
    )]
    public class IdentityServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            
            context.Services.AddAutoMapperObjectMapper<IdentityServiceApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityServiceApplicationModule>(validate: true);
            });
            
            Configure<KeycloakClientOptions>(options =>
                {
                    options.Url = configuration["Keycloak:url"];
                    options.AdminUserName = configuration["Keycloak:adminUsername"];
                    options.AdminPassword = configuration["Keycloak:adminPassword"];
                    options.RealmName = configuration["Keycloak:realmName"];
                }
            );
        }
    }
}