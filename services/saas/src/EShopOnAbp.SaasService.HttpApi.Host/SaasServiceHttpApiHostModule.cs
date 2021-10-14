using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EShopOnAbp.SaasService.DbMigrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EShopOnAbp.SaasService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.SaasService
{
    [DependsOn(
        typeof(EShopOnAbpSharedHostingMicroservicesModule),
        typeof(SaasServiceEntityFrameworkCoreModule),
        typeof(SaasServiceApplicationModule),
        typeof(SaasServiceHttpApiModule)
    )]
    public class SaasServiceHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddAbpDbContext<SaasServiceDbContext>();

            JwtBearerConfigurationHelper.Configure(context, "SaasService");
            // SwaggerConfigurationHelper.Configure(context, "Saas Service API");

            SwaggerWithAuthConfigurationHelper.Configure(
                context: context,
                authority: configuration["AuthServer:Authority"],
                scopes: new
                    Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                    {
                        { "SaasService", "Saas Service API" },
                    },
                apiTitle: "Saas Service API"
            );

            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.Trim().RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
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
            app.UseCors();
            app.UseAbpRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            // app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.UseMultiTenancy();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Saas Service API"); });
            app.UseAbpSerilogEnrichers();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConsul();
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
                        .GetRequiredService<SaasServiceDatabaseMigrationChecker>()
                        .CheckAsync()
                );
            }
        }
    }

    public class Test
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}