using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.Microservices
{
    public static class SwaggerConfigurationHelper
    {
        public static void Configure(
            ServiceConfigurationContext context,
            string apiTitle
        )
        {
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = apiTitle, Version = "v1"});
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        }
    }
}