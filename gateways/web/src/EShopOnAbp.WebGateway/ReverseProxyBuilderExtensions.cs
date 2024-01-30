using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using EShopOnAbp.WebGateway.Aggregations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Yarp.ReverseProxy.Configuration;

namespace EShopOnAbp.WebGateway;

public static class ReverseProxyBuilderExtensions
{
    public static string LocalizationPath = "/api/abp/application-localization?cultureName=tr&onlyDynamics=false";

    public static ReverseProxyConventionBuilder MapReverseProxyWithLocalization(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapReverseProxy(proxyBuilder =>
        {
            proxyBuilder.Use(async (context, next) =>
            {
                var endpoint = context.GetEndpoint();

                var localizationAggregation = context.RequestServices.GetRequiredService<ILocalizationAggregation>();
                if (localizationAggregation.LocalizationEndpoint == endpoint?.DisplayName)
                {
                    LocalizationRequest input = CreateLocalizationRequestInput(context);

                    var result = await localizationAggregation.GetLocalizationAsync(input);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    }));
                    return;
                }

                await next();
            });

            proxyBuilder.UseLoadBalancing();
        });
    }

    private static LocalizationRequest CreateLocalizationRequestInput(HttpContext context)
    {
        var proxyConfig = context.RequestServices.GetRequiredService<IProxyConfigProvider>();

        context.Request.Query.TryGetValue("CultureName", out var cultureName);

        var input = new LocalizationRequest(cultureName);

        var clusterList = GetClusters(proxyConfig);
        foreach (var cluster in clusterList)
        {
            var hostUrl = new Uri(cluster.Value.Address) + $"{LocalizationPath}";
            input.LocalizationEndpoints.Add($"{cluster.Key}_{cultureName}", hostUrl);
        }

        return input;
    }

    private static Dictionary<string, DestinationConfig> GetClusters(IProxyConfigProvider proxyConfig)
    {
        var yarpConfig = proxyConfig.GetConfig();

        var routedClusters = yarpConfig.Clusters
            .SelectMany(t => t.Destinations,
                (clusterId, destination) => new { clusterId.ClusterId, destination.Value });

        return routedClusters
            .GroupBy(q => q.Value.Address)
            .Select(t => t.First())
            .Distinct()
            .ToDictionary(k => k.ClusterId, v => v.Value);
    }
}