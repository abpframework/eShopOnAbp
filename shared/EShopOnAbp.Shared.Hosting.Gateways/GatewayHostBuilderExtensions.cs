using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class AbpHostingHostBuilderExtensions
{
    public const string AppOcelotJsonPath = "ocelot.json";

    public static IHostBuilder AddOcelotJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true,
        string path = AppOcelotJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            builder.AddJsonFile(
                path: AppOcelotJsonPath,
                optional: optional,
                reloadOnChange: reloadOnChange
            );
        });
    }
}