using System;
using System.Linq;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Yarp.ReverseProxy.Configuration;

namespace EShopOnAbp.Shared.Hosting.Gateways;

public static class YarpSwaggerUIBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerUIWithYarp(this IApplicationBuilder app,
        ApplicationInitializationContext context)
    {
        app.UseSwagger();
        app.UseAbpSwaggerWithCustomScriptUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = context.ServiceProvider.GetRequiredService<ILogger<ApplicationInitializationContext>>();
            var proxyConfigProvider = context.ServiceProvider.GetRequiredService<IProxyConfigProvider>();
            var yarpConfig = proxyConfigProvider.GetConfig();

            var routedClusters = yarpConfig.Clusters
                .SelectMany(t => t.Destinations,
                    (clusterId, destination) => new {clusterId.ClusterId, destination.Value});

            var groupedClusters = routedClusters
                .GroupBy(q => q.Value.Address)
                .Select(t => t.First())
                .Distinct()
                .ToList();

            foreach (var clusterGroup in groupedClusters)
            {
                var routeConfig = yarpConfig.Routes.FirstOrDefault(q =>
                    q.ClusterId == clusterGroup.ClusterId);
                if (routeConfig == null)
                {
                    logger.LogWarning($"Swagger UI: Couldn't find route configuration for {clusterGroup.ClusterId}...");
                    continue;
                }

                var baseUrl = clusterGroup.Value.Address;

                if (Convert.ToBoolean(configuration["App:IsOnK8s"])) // If the application is running on K8s, the swagger.json should be reached from public dns.
                {
                    baseUrl = clusterGroup.Value.Metadata?["PublicAddress"];
                }

                options.SwaggerEndpoint($"{baseUrl}/swagger/v1/swagger.json", $"{routeConfig.RouteId} API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            }
        });
        
        return app;
    }
}