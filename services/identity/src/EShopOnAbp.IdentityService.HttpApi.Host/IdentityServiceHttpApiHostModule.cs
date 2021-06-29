using EShopOnAbp.IdentityService.DbMigrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EShopOnAbp.IdentityService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace EShopOnAbp.IdentityService
{
    [DependsOn(
        typeof(EShopOnAbpSharedHostingMicroservicesModule),
        typeof(IdentityServiceHttpApiModule),
        typeof(IdentityServiceApplicationModule),
        typeof(IdentityServiceEntityFrameworkCoreModule)
    )]
    public class IdentityServiceHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            JwtBearerConfigurationHelper.Configure(context, "IdentityService");
            SwaggerConfigurationHelper.Configure(context, "Identity Service API");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId();
            app.UseAbpRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            // app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.UseMultiTenancy();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API");
            });
            app.UseAbpSerilogEnrichers();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints(endpoints =>
            {
                // endpoints.MapMetrics();
            });
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(
                    () => scope.ServiceProvider
                        .GetRequiredService<IdentityServiceDatabaseMigrationChecker>()
                        .CheckAsync()
                );
            }
        }
    }
}