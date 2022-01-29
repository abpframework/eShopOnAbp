using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EShopOnAbp.CatalogService;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information($"Starting {assemblyName}.");
            // var app = await ApplicationBuilderHelper
            //     .BuildApplicationAsync<CatalogServiceHttpApiHostModule>(args);

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureEndpointDefaults(listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
                serverOptions.ListenLocalhost(44354, opts =>
                {
                    opts.UseHttps();
                    opts.Protocols = HttpProtocols.Http1AndHttp2;
                });
                serverOptions.ListenLocalhost(81, opts => { opts.Protocols = HttpProtocols.Http2; });
            });
            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();

            await builder.AddApplicationAsync<CatalogServiceHttpApiHostModule>();
            var app = builder.Build();

            await app.InitializeApplicationAsync();
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
    // {
    //     var grpcPort = config.GetValue("GRPC_PORT", 81);
    //     var port = config.GetValue("PORT", 80);
    //     return (port, grpcPort);
    // }
    //
    // static IConfiguration GetConfiguration()
    // {
    //     return new ConfigurationBuilder()
    //         .SetBasePath(Directory.GetCurrentDirectory())
    //         .AddJsonFile("appsettings.json")
    //         .AddEnvironmentVariables()
    //         .Build();
    // }
}