using System;
using System.Collections.Generic;
using System.Linq;
using EShopOnAbp.WebGateway.Aggregations.ApplicationConfiguration;
using EShopOnAbp.WebGateway.Aggregations.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace EShopOnAbp.WebGateway;

public static class ReverseProxyBuilderExtensions
{
    public static ReverseProxyConventionBuilder MapReverseProxyWithLocalization(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapReverseProxy(proxyBuilder =>
        {
            proxyBuilder.Use(async (context, next) =>
            {
                var endpoint = context.GetEndpoint();

                var localizationAggregation = context.RequestServices
                    .GetRequiredService<ILocalizationAggregation>();

                var appConfigurationAggregation = context.RequestServices
                    .GetRequiredService<IAppConfigurationAggregation>();

                // The "/api/abp/application-localization" endpoint
                if (localizationAggregation.LocalizationRouteName == endpoint?.DisplayName)
                {
                    var localizationRequestInput =
                        CreateLocalizationRequestInput(context, localizationAggregation.LocalizationEndpoint);

                    var result = await localizationAggregation.GetLocalizationAsync(localizationRequestInput);
                    await context.Response.WriteAsJsonAsync(result);
                    return;
                }

                // The "/api/abp/application-configuration" endpoint
                if (appConfigurationAggregation.AppConfigRouteName == endpoint?.DisplayName)
                {
                    var appConfigurationRequestInput =
                        CreateAppConfigurationRequestInput(context, appConfigurationAggregation.AppConfigEndpoint);

                    var result =
                        await appConfigurationAggregation.GetAppConfigurationAsync(appConfigurationRequestInput);
                    await context.Response.WriteAsJsonAsync(result);
                    return;
                }

                await next();
            });

            proxyBuilder.UseLoadBalancing();
        });
    }

    private static AppConfigurationRequest CreateAppConfigurationRequestInput(HttpContext context,
        string appConfigurationPath)
    {
        var proxyConfig = context.RequestServices.GetRequiredService<IProxyConfigProvider>();

        var input = new AppConfigurationRequest();
        string path = $"{appConfigurationPath}?includeLocalizationResources=false";

        var clusterList = GetClusters(proxyConfig);
        foreach (var cluster in clusterList)
        {
            var hostUrl = new Uri(cluster.Value.Address) + $"{path}";
            // CacheKey/Endpoint dictionary key -> ex: ("Administration_AppConfig")
            input.Endpoints.Add($"{cluster.Key}_AppConfig", hostUrl);
        }

        return input;
    }

    private static LocalizationRequest CreateLocalizationRequestInput(HttpContext context, string localizationPath)
    {
        var proxyConfig = context.RequestServices.GetRequiredService<IProxyConfigProvider>();

        context.Request.Query.TryGetValue("CultureName", out var cultureName);

        var input = new LocalizationRequest(cultureName);
        string path = $"{localizationPath}?cultureName={cultureName}&onlyDynamics=false";

        var clusterList = GetClusters(proxyConfig);
        foreach (var cluster in clusterList)
        {
            var hostUrl = new Uri(cluster.Value.Address) + $"{path}";
            // Endpoint dictionary key -> ex: ("Administration_en")
            input.Endpoints.Add($"{cluster.Key}_{cultureName}", hostUrl);
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