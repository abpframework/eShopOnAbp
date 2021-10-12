using EShopOnAbp.PaymentService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using System.Collections.Generic;
using System;
using EShopOnAbp.PaymentService.DbMigrations;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace EShopOnAbp.PaymentService
{
    [DependsOn(
        typeof(PaymentServiceApplicationModule),
        typeof(PaymentServiceEntityFrameworkCoreModule),
        typeof(PaymentServiceHttpApiModule),
        typeof(EShopOnAbpSharedHostingMicroservicesModule)
        )]
    public class PaymentServiceHttpApiHostModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            JwtBearerConfigurationHelper.Configure(context, "CatalogService");
            // SwaggerConfigurationHelper.Configure(context, "Catalog Service API");

            SwaggerWithAuthConfigurationHelper.Configure(
                context: context,
                authority: configuration["AuthServer:Authority"],
                scopes: new Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"CatalogService", "Catalog Service API"},
                },
                apiTitle: "Catalog Service API"
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
                        .GetRequiredService<PaymentServiceDatabaseMigrationChecker>()
                        .CheckAsync()
                );
            }
        }
    }
}
