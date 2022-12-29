using EShopOnAbp.BasketService;
using EShopOnAbp.CatalogService;
using EShopOnAbp.CmskitService;
using EShopOnAbp.Localization;
using EShopOnAbp.OrderingService;
using EShopOnAbp.PaymentService;
using EShopOnAbp.PaymentService.PaymentMethods;
using EShopOnAbp.PublicWeb.AnonymousUser;
using EShopOnAbp.PublicWeb.Components.Toolbar.Cart;
using EShopOnAbp.PublicWeb.Menus;
using EShopOnAbp.PublicWeb.PaymentMethods;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Polly;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit;
using Volo.CmsKit.Public.Web;
using Yarp.ReverseProxy.Transforms;

namespace EShopOnAbp.PublicWeb;

[DependsOn(
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(AbpAspNetCoreMvcClientModule),
    typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
    typeof(AbpHttpClientIdentityModelWebModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountHttpApiClientModule),
    typeof(EShopOnAbpSharedHostingAspNetCoreModule),
    typeof(EShopOnAbpSharedLocalizationModule),
    typeof(CatalogServiceHttpApiClientModule),
    typeof(BasketServiceContractsModule),
    typeof(OrderingServiceHttpApiClientModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(PaymentServiceHttpApiClientModule),
    typeof(AbpAutoMapperModule),
    typeof(CmskitServiceHttpApiClientModule),
    typeof(CmsKitPublicWebModule)
)]
public class EShopOnAbpPublicWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(EShopOnAbpResource),
                typeof(EShopOnAbpPublicWebModule).Assembly
            );
        });

        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
            {
                clientBuilder.AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(
                        3,
                        i => TimeSpan.FromSeconds(Math.Pow(2, i))
                    )
                );
            });
        });

        FeatureConfigurer.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        var configuration = context.Services.GetConfiguration();

        ConfigureBasketHttpClient(context);

        context.Services.AddAutoMapperObjectMapper<EShopOnAbpPublicWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<EShopOnAbpPublicWebModule>(validate: true); });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle => { 
                    bundle.AddContributors(typeof(CartWidgetStyleContributor));
					bundle.AddFiles("/global.css");
				}
            );
        });

        context.Services.Configure<AbpRemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default =
                new RemoteServiceConfiguration(configuration["RemoteServices:Default:BaseUrl"]);
        });

        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = true; });

        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "EShopOnAbp:"; });

        Configure<AppUrlOptions>(options => { options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"]; });

        ConfigurePayment(configuration);

        context.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddAbpOpenIdConnect("oidc", options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.ClientId = configuration["AuthServer:ClientId"];
                options.MetadataAddress = configuration["AuthServer:MetaAddress"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("phone");
                options.Scope.Add("roles");
                options.Scope.Add("offline_access");

                options.Scope.Add("AdministrationService");
                options.Scope.Add("BasketService");
                options.Scope.Add("IdentityService");
                options.Scope.Add("CatalogService");
                options.Scope.Add("PaymentService");
                options.Scope.Add("OrderingService");
                options.Scope.Add("CmskitService");

                options.SaveTokens = true;

                //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
                // options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                // options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
                //
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true
                };

                options.Events.OnAuthorizationCodeReceived = async (authContext) =>
                {
                    var userLoggedInEto = CreateUserLoggedInEto(authContext.Principal, authContext.HttpContext);
                    if (userLoggedInEto != null)
                    {
                        var eventBus =
                            authContext.HttpContext.RequestServices.GetRequiredService<IDistributedEventBus>();
                        await eventBus.PublishAsync(userLoggedInEto);
                    }
                };

                if (AbpClaimTypes.UserName != "preferred_username")
                {
                    options.ClaimActions.MapJsonKey(AbpClaimTypes.UserName, "preferred_username");
                    options.ClaimActions.DeleteClaim("preferred_username");
                    options.ClaimActions.RemoveDuplicate(AbpClaimTypes.UserName);
                }
            });

        if (Convert.ToBoolean(configuration["AuthServer:IsOnProd"]))
        {
            context.Services.Configure<OpenIdConnectOptions>("oidc", options =>
            {
                options.MetadataAddress = configuration["AuthServer:MetaAddress"].EnsureEndsWith('/') +
                                          ".well-known/openid-configuration";

                var previousOnRedirectToIdentityProvider = options.Events.OnRedirectToIdentityProvider;
                options.Events.OnRedirectToIdentityProvider = async ctx =>
                {
                    // Intercept the redirection so the browser navigates to the right URL in your host
                    ctx.ProtocolMessage.IssuerAddress = configuration["AuthServer:Authority"].EnsureEndsWith('/') +
                                                        "connect/authorize";

                    if (previousOnRedirectToIdentityProvider != null)
                    {
                        await previousOnRedirectToIdentityProvider(ctx);
                    }
                };
                var previousOnRedirectToIdentityProviderForSignOut =
                    options.Events.OnRedirectToIdentityProviderForSignOut;
                options.Events.OnRedirectToIdentityProviderForSignOut = async ctx =>
                {
                    // Intercept the redirection for signout so the browser navigates to the right URL in your host
                    ctx.ProtocolMessage.IssuerAddress = configuration["AuthServer:Authority"].EnsureEndsWith('/') +
                                                        "connect/endsession";

                    if (previousOnRedirectToIdentityProviderForSignOut != null)
                    {
                        await previousOnRedirectToIdentityProviderForSignOut(ctx);
                    }
                };
            });
        }

        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "EShopOnAbp-Protection-Keys")
            .SetApplicationName("eShopOnAbp-PublicWeb");

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new EShopOnAbpPublicWebMenuContributor(configuration));
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new EShopOnAbpPublicWebToolbarContributor());
        });

        context.Services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(async (transformContext) =>
                {
                    transformContext.ProxyRequest.Headers
                        .Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        await transformContext.HttpContext.GetTokenAsync("access_token")
                    );
                });
            });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.Use((ctx, next) =>
        {
            ctx.Request.Scheme = "https";
            return next();
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        // app.UseHttpMetrics();
        app.UseAuthentication();
        app.UseAbpSerilogEnrichers();
        app.UseAuthorization();
        app.UseAnonymousUser();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
            // endpoints.MapMetrics();
        });
    }

    private void ConfigureBasketHttpClient(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(BasketServiceContractsModule).Assembly,
            remoteServiceConfigurationName: BasketServiceConstants.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EShopOnAbpPublicWebModule>();
        });
    }

    private void ConfigurePayment(IConfiguration configuration)
    {
        Configure<EShopOnAbpPublicWebPaymentOptions>(options =>
        {
            options.PaymentSuccessfulCallbackUrl =
                configuration["App:SelfUrl"].EnsureEndsWith('/') + "PaymentCompleted";
        });

        Configure<PaymentMethodUiOptions>(options =>
        {
            options.ConfigureIcon(PaymentMethodNames.PayPal, "fa-cc-paypal paypal");
        });
    }

    private UserLoggedInEto CreateUserLoggedInEto(ClaimsPrincipal principal, HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<ILogger<EShopOnAbpPublicWebModule>>();

        if (principal == null)
        {
            logger.LogWarning($"AuthorizationCode does not contain principal to create/update user!");
            return null;
        }

        var claims = principal.Claims.ToList();

        var userNameClaim = claims.FirstOrDefault(x => x.Type == "preferred_username");
        var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        var isEmailVerified = claims.FirstOrDefault(x => x.Type == "email_verified")?.Value == "true";
        var phoneNumberClaim = claims.FirstOrDefault(x => x.Type == "phone");
        var userIdString = claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value;

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            logger.LogWarning(
                $"Handling UserLoggedInEvent... User creation failed! {userIdString} can not be parsed!");
            return null;
        }

        return new UserLoggedInEto
        {
            Id = userId,
            Email = emailClaim?.Value,
            UserName = userNameClaim?.Value,
            Phone = phoneNumberClaim?.Value,
            IsEmailVerified = isEmailVerified
        };
    }
}