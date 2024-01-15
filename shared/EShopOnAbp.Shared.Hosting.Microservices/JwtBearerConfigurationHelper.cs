using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.Modularity;

namespace EShopOnAbp.Shared.Hosting.Microservices;

public static class JwtBearerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,
        string audience)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = audience;
                // IDX10204: Unable to validate issuer on K8s if not set
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuers = new[]
                        { configuration["AuthServer:Authority"], configuration["AuthServer:MetadataAddress"] },
                    // IDX10500: Signature validation failed. No security keys were provided to validate the signature on K8s
                    SignatureValidator = delegate(string token, TokenValidationParameters parameters)
                    {
                        var jwt = new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token);
                        return jwt;
                    }
                };
            });
    }
}