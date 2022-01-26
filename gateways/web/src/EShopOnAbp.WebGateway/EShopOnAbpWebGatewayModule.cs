using EShopOnAbp.Shared.Hosting.Gateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WebGateway;

[DependsOn(
    typeof(EShopOnAbpSharedHostingGatewaysModule)
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
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"AccountService", "Account Service API"},
                    {"IdentityService", "Identity Service API"},
                    {"AdministrationService", "Administration Service API"},
                    {"CatalogService", "Catalog Service API"},
                    {"BasketService", "Basket Service API"},
                    {"PaymentService", "Payment Service API"},
                    {"OrderingService", "Ordering Service API"},
                },
            apiTitle: "Web Gateway"
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
        app.UseAbpSerilogEnrichers();
        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var routes = configuration.GetSection("Routes").Get<List<OcelotConfiguration>>();
            var routedServices = routes
                .GroupBy(t => t.ServiceKey)
                .Select(r => r.First())
                .Distinct();

            foreach (var config in routedServices)
            {
                var url =
                    $"{config.DownstreamScheme}://{config.DownstreamHostAndPorts.FirstOrDefault()?.Host}:{config.DownstreamHostAndPorts.FirstOrDefault()?.Port}";
                if (!env.IsDevelopment())
                {
                    url = $"https://{config.DownstreamHostAndPorts.FirstOrDefault()?.Host}";
                }

                options.SwaggerEndpoint($"{url}/swagger/v1/swagger.json", $"{config.ServiceKey} API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            }
        });
        app.MapWhen(
            ctx => ctx.Request.Path.ToString().TrimEnd('/').Equals(""),
            app2 =>
            {
                app2.UseRouting();
                app2.UseConfiguredEndpoints();
            }
        );
        app.UseOcelot().Wait();
    }
}