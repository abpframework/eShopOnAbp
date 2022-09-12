using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using EShopOnAbp.CmskitService.EntityFrameworkCore;
using EShopOnAbp.Shared.Hosting.Microservices;
using Volo.Abp;
using Volo.Abp.Modularity;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;

namespace EShopOnAbp.CmskitService;
[DependsOn(
    typeof(EShopOnAbpSharedHostingMicroservicesModule),
    typeof(CmskitServiceApplicationModule),
    typeof(CmskitServiceHttpApiModule),
    typeof(CmskitServiceEntityFrameworkCoreModule)
    )]
public class CmskitServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
         var configuration = context.Services.GetConfiguration();
        // var hostingEnvironment = context.Services.GetHostingEnvironment();

        JwtBearerConfigurationHelper.Configure(context, "CmskitService");

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    { "CmskitService", "CmskitService Service API" }
                },
            apiTitle: "CmskitService Service API"
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

        // TODO: Create controller instead of auto-controller configuration
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CmskitServiceApplicationModule).Assembly, opts =>
            {
                opts.RootPath = "cmskit";
                opts.RemoteServiceName = "Cmskit";
            });
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
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cmskit Service API");
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }
}
