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
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace EShopOnAbp.WebGateway
{
    [DependsOn(
    typeof(EShopOnAbpSharedHostingGatewaysModule)
)]
    public class EShopOnAbpWebGatewayModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            SwaggerConfigurationHelper.Configure(context, "Web Gateway");
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
                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                var routes = configuration.GetSection("Routes").Get<List<OcelotConfiguration>>();
                
                foreach (var config in routes.GroupBy(t => t.ServiceKey).Select(r => r.First()).Distinct())
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
