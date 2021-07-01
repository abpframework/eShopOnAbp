using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.Gateways
{
    public static class SwaggerWithAuthConfigurationHelper
    {
        public static void Configure(
            ServiceConfigurationContext context,
            string authority,
            Dictionary<string, string> scopes,
            string apiTitle,
            string apiVersion = "v1",
            string apiName = "v1"
        )
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                authority: authority,
                scopes: scopes,
                options =>
                {
                    options.SwaggerDoc(apiName, new OpenApiInfo { Title = apiTitle, Version = apiVersion });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
        }
    }
}