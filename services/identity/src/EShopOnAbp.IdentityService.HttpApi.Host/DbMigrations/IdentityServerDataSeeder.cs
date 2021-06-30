using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace EShopOnAbp.IdentityService.DbMigrations
{
    public class IdentityServerDataSeeder : ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;

        public IdentityServerDataSeeder(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration,
            ICurrentTenant currentTenant)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync()
        {
            using (_currentTenant.Change(null))
            {
                await _identityResourceDataSeeder.CreateStandardResourcesAsync();
                await CreateApiResourcesAsync();
                await CreateApiScopesAsync();
                await CreateSwaggerClientsAsync();
                await CreateClientsAsync();
            }
        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

            await CreateApiResourceAsync("IdentityService", commonApiUserClaims);
            await CreateApiResourceAsync("AdministrationService", commonApiUserClaims);
            await CreateApiResourceAsync("SaasService", commonApiUserClaims);
        }

        private async Task CreateApiScopesAsync()
        {
            await CreateApiScopeAsync("IdentityService");
            await CreateApiScopeAsync("AdministrationService");
            await CreateApiScopeAsync("SaasService");
        }

        private async Task CreateSwaggerClientsAsync()
        {
            await CreateSwaggerClientAsync("InternalGateway",
                new[] { "IdentityService", "AdministrationService", "SaasService" });
            await CreateSwaggerClientAsync("WebGateway",
                new[] { "IdentityService", "AdministrationService", "SaasService" });
            await CreateSwaggerClientAsync("WebPublicGateway" );
        }

        private async Task CreateSwaggerClientAsync(string name, string[] scopes = null)
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address"
            };
            scopes ??= new[] { name };

            // Swagger Client
            var swaggerClientId = $"{name}_Swagger";
            if (!swaggerClientId.IsNullOrWhiteSpace())
            {
                var swaggerRootUrl = _configuration[$"IdentityServerClients:{name}:RootUrl"].TrimEnd('/');

                await CreateClientAsync(
                    name: swaggerClientId,
                    scopes: commonScopes.Union(scopes),
                    grantTypes: new[] { "authorization_code" },
                    secret: "1q2w3e*".Sha256(),
                    requireClientSecret: false,
                    redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                    corsOrigins: new[] { swaggerRootUrl.RemovePostFix("/") }
                );
            }
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(
                    new ApiScope(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            return apiScope;
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address"
            };

            //Public Web Client
            var publicWebClientRootUrl = _configuration["IdentityServerClients:EShopOnAbp_PublicWeb:RootUrl"]
                .EnsureEndsWith('/');
            await CreateClientAsync(
                name: "EShopOnAbp_PublicWeb",
                scopes: commonScopes.Union(new[]
                {
                    "AdministrationService"
                }),
                grantTypes: new[] { "hybrid" },
                secret: "1q2w3e*".Sha256(),
                redirectUri: $"{publicWebClientRootUrl}signin-oidc",
                postLogoutRedirectUri: $"{publicWebClientRootUrl}signout-callback-oidc",
                frontChannelLogoutUri: $"{publicWebClientRootUrl}Account/FrontChannelLogout",
                corsOrigins: new[] { publicWebClientRootUrl.RemovePostFix("/") }
            );

            //Angular Client
            var angularClientRootUrl =
                _configuration["IdentityServerClients:EShopOnAbp_Angular:RootUrl"].TrimEnd('/');
            await CreateClientAsync(
                name: "EShopOnAbp_Angular",
                scopes: commonScopes.Union(new[]
                {
                    "IdentityService",
                    "AdministrationService",
                    "SaasService"
                }),
                grantTypes: new[] { "authorization_code", "LinkLogin" },
                secret: "1q2w3e*".Sha256(),
                requirePkce: true,
                requireClientSecret: false,
                redirectUri: $"{angularClientRootUrl}",
                postLogoutRedirectUri: $"{angularClientRootUrl}",
                corsOrigins: new[] { angularClientRootUrl }
            );

            //Administration Service Client
            await CreateClientAsync(
                name: "EShopOnAbp_AdministrationService",
                scopes: commonScopes.Union(new[]
                {
                    "IdentityService"
                }),
                grantTypes: new[] { "client_credentials" },
                secret: "1q2w3e*".Sha256(),
                permissions: new[] { IdentityPermissions.Users.Default }
            );
        }

        private async Task<Client> CreateClientAsync(
            string name,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null,
            string redirectUri = null,
            string postLogoutRedirectUri = null,
            string frontChannelLogoutUri = null,
            bool requireClientSecret = true,
            bool requirePkce = false,
            IEnumerable<string> permissions = null,
            IEnumerable<string> corsOrigins = null)
        {
            var client = await _clientRepository.FindByClientIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client(
                        _guidGenerator.Create(),
                        name
                    )
                    {
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 31536000, //365 days
                        AccessTokenLifetime = 31536000, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false,
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        RequireClientSecret = requireClientSecret,
                        RequirePkce = requirePkce
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions,
                    null
                );
            }

            if (corsOrigins != null)
            {
                foreach (var origin in corsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}