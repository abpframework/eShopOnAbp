using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class AbpHostingHostBuilderExtensions
{
    public const string AppYarpJsonPath = "yarp.json";

    public static IHostBuilder AddYarpJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true,
        string path = AppYarpJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            string environmentName = context.HostingEnvironment.EnvironmentName;
            string yarpJsonPath = environmentName == "Tye" ? "yarp.Tye.json" : AppYarpJsonPath;
            builder.AddJsonFile(
                    path: yarpJsonPath,
                    optional: optional,
                    reloadOnChange: reloadOnChange
                )
                .AddEnvironmentVariables();
        });
    }
}