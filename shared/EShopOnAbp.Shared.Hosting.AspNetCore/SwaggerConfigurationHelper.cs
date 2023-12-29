using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.AspNetCore;

public static class SwaggerConfigurationHelper
{
    public static void ConfigureWithOidc(
        ServiceConfigurationContext context,
        string authority,
        string[] scopes,
        string apiTitle,
        string apiVersion = "v1",
        string apiName = "v1",
        string[]? flows = null,
        string? discoveryEndpoint = null
    )
    {
        context.Services.AddAbpSwaggerGenWithOidc(
            authority: authority,
            scopes: scopes,
            flows: flows,
            discoveryEndpoint: discoveryEndpoint,
            options =>
            {
                options.SwaggerDoc(apiName, new OpenApiInfo { Title = apiTitle, Version = apiVersion });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }
}