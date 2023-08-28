using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.Shared.Hosting.Gateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WebPublicGateway;

[DependsOn(
    typeof(EShopOnAbpSharedHostingGatewaysModule)
)]
public class EShopOnAbpWebPublicGatewayModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        SwaggerConfigurationHelper
            .ConfigureWithOidc(
                context: context,
                authority: configuration["AuthServer:Authority"]!,
                /* Requested scopes for authorization code request and descriptions for swagger UI only */
                scopes: new[]
                {
                    "AccountService", "IdentityService", "AdministrationService", "CatalogService", "BasketService",
                    "OrderingService", "CmskitService"
                },
                apiTitle: "Public Web Gateway API",
                discoveryEndpoint: configuration["AuthServer:MetadataAddress"]
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
        app.UseAbpSerilogEnrichers();
        app.UseSwaggerUIWithYarp(context);

        app.UseRewriter(new RewriteOptions()
            // Regex for "", "/" and "" (whitespace)
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("", ctx => ctx.Response.WriteAsync("YAG"));
            endpoints.MapReverseProxy();
        });
    }
}