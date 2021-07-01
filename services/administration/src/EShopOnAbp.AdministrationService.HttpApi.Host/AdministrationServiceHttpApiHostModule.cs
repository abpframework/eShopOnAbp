using EShopOnAbp.AdministrationService.DbMigrations;
using EShopOnAbp.AdministrationService.EntityFrameworkCore;
using EShopOnAbp.SaasService;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.AdministrationService
{
    [DependsOn(
        typeof(AdministrationServiceHttpApiModule),
        typeof(AdministrationServiceApplicationModule),
        typeof(AdministrationServiceEntityFrameworkCoreModule),
        typeof(EShopOnAbpSharedHostingMicroservicesModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(SaasServiceApplicationContractsModule),
        typeof(AbpIdentityHttpApiClientModule)
    )]
    public class AdministrationServiceHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            JwtBearerConfigurationHelper.Configure(context, "AdministrationService");
            SwaggerConfigurationHelper.Configure(context, "Administration Service API");
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
            //app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.UseMultiTenancy();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Administration Service API");
            });
            app.UseAbpSerilogEnrichers();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints(endpoints =>
            {
                //endpoints.MapMetrics();
            });
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(
                    () => scope.ServiceProvider
                        .GetRequiredService<AdministrationServiceDatabaseMigrationChecker>()
                        .CheckAsync()
                );

                //Log.Information("Sending event...");

                //AsyncHelper.RunSync(
                //    () => scope.ServiceProvider
                //        .GetRequiredService<IDistributedEventBus>()
                //        .PublishAsync(new TenantCreatedEto { Id = Guid.Empty, Name = "Sample" })
                //    );
            }
        }
    }
}
