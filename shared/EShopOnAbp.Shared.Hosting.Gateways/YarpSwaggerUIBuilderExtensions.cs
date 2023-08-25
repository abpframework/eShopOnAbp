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
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = context.ServiceProvider.GetRequiredService<ILogger<ApplicationInitializationContext>>();
            var proxyConfig = context.ServiceProvider.GetRequiredService<IProxyConfigProvider>().GetConfig();

            foreach (var cluster in proxyConfig.Clusters)
            {
                options.SwaggerEndpoint($"/swagger-json/{cluster.ClusterId}/swagger/v1/swagger.json",
                    $"{cluster.ClusterId} API");
            }
        });
        
        return app;
    }
}