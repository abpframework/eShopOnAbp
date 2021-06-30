using System.Collections.Generic;
using EShopOnAbp.AdministrationService;
using EShopOnAbp.IdentityService;
using EShopOnAbp.SaasService;
using EShopOnAbp.Shared.Hosting.Gateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Middleware;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WebGateway
{
    [DependsOn(
    typeof(EShopOnAbpSharedHostingGatewaysModule),
    typeof(IdentityServiceHttpApiModule),
    typeof(SaasServiceHttpApiModule),
    typeof(AdministrationServiceHttpApiModule)
)]
    public class EShopOnAbpWebGatewayModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            SwaggerWithAuthConfigurationHelper.Configure(
                context: context,
                authority: configuration["AuthServer:Authority"],
                scopes: new Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                    {
                      {"IdentityService", "Identity Service API"},
                      {"AdministrationService", "Administration Service API"},
                      {"SaasService", "Saas Service API"},
                    },
                apiTitle: "Web Gateway API"
            );
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
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Gateway API");
                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            });
            app.UseAbpSerilogEnrichers();
            app.MapWhen(
                ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/api-definition") ||
                       ctx.Request.Path.ToString().TrimEnd('/').Equals(""),
                app2 =>
                {
                    app2.UseRouting();
                    app2.UseConfiguredEndpoints();
                }
            );
            app.UseOcelot().Wait();
        }
    }
}
