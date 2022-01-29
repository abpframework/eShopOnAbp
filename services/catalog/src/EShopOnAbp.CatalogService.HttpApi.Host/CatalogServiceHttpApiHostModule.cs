using EShopOnAbp.CatalogService.DbMigrations;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.CatalogService.Grpc;
using EShopOnAbp.CatalogService.MongoDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace EShopOnAbp.CatalogService;

[DependsOn(
    typeof(CatalogServiceHttpApiModule),
    typeof(CatalogServiceApplicationModule),
    typeof(CatalogServiceMongoDbModule),
    typeof(EShopOnAbpSharedHostingMicroservicesModule)
)]
public class CatalogServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        JwtBearerConfigurationHelper.Configure(context, "CatalogService");

        SwaggerWithAuthConfigurationHelper.Configure(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    { "CatalogService", "Catalog Service API" }
                },
            apiTitle: "Catalog Service API"
        );

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CatalogServiceApplicationModule).Assembly, opts =>
            {
                opts.RootPath = "catalog";
                opts.RemoteServiceName = "Catalog";
            });
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            //Standalone MongoDB servers don't support transactions
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });

        Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });
        context.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseCors();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API");
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<PublicProductGrpService>();
            endpoints.MapGet("/_proto/", async ctx =>
            {
                ctx.Response.ContentType = "text/plain";
                using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Protos", "product.proto"), FileMode.Open, FileAccess.Read);
                using var sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (line != "/* >>" || line != "<< */")
                    {
                        await ctx.Response.WriteAsync(line);
                    }
                }
            });
        });
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<CatalogServiceDatabaseMigrationChecker>()
            .CheckAsync();
    }
}