using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.ClientScopes;
using Keycloak.Net.Models.ProtocolMappers;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.DbMigrator;

public class KeyCloakDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;

    public KeyCloakDataSeeder(IOptions<KeycloakClientOptions> keycloakClientOptions)
    {
        _keycloakOptions = keycloakClientOptions.Value;

        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await UpdateAdminUserAsync();
        await CreateClientScopesAsync();
        await CreateClientsAsync();
    }

    private async Task CreateClientScopesAsync()
    {
        await CreateScopeAsync("AdministrationService");
        await CreateScopeAsync("IdentityService");
        await CreateScopeAsync("BasketService");
        await CreateScopeAsync("CatalogService");
        await CreateScopeAsync("OrderingService");
        await CreateScopeAsync("PaymentService");
        await CreateScopeAsync("CmskitService");
    }

    private async Task CreateScopeAsync(string scopeName)
    {
        var scope = (await _keycloakClient.GetClientScopesAsync(_keycloakOptions.RealmName))
            .FirstOrDefault(q => q.Name == scopeName);
        
        if (scope == null)
        {
            scope = new ClientScope()
            {
                Name = scopeName,
                Description = scopeName + " scope",
                Protocol = "openid-connect",
                Attributes = new Attributes
                {
                    ConsentScreenText = scopeName,
                    DisplayOnConsentScreen = "true",
                    IncludeInTokenScope = "true"
                },
                ProtocolMappers = new List<ProtocolMapper>()
                {
                    new ProtocolMapper()
                    {
                        Name = scopeName,
                        Protocol = "openid-connect",
                        _ProtocolMapper = "oidc-audience-mapper",
                        // Config = new Dictionary<string, string>()
                        // {
                        //     {"id.token.claim", "false"},
                        //     {"access.token.claim", "true"},
                        //     {"included.custom.audience", scopeName}
                        // }
                        Config = new Config() // This should be dictionary -> Outdated library
                        {
                            AccessTokenClaim = "true",
                            IdTokenClaim = "false"
                        }
                    }
                }
            };

            await _keycloakClient.CreateClientScopeAsync(_keycloakOptions.RealmName, scope);
        }
    }

    private async Task CreateClientsAsync()
    {
        await CreatePublicWebClientAsync();
    }

    private async Task CreatePublicWebClientAsync()
    {
        var publicWebClient = (await _keycloakClient.GetClientsAsync(_keycloakOptions.RealmName, clientId: "PublicWeb"))
            .FirstOrDefault();

        if (publicWebClient == null)
        {
            publicWebClient = new Client()
            {
                ClientId = "PublicWeb",
                Name = "Public Web Application",
                Protocol = "openid-connect",
                Enabled = true,
                BaseUrl = "https://localhost:44335/",
                RedirectUris = new List<string>
                {
                    "https://localhost:44335/signin-oidc"
                },
                FrontChannelLogout = true,
                PublicClient = true
            };
            publicWebClient.Attributes = new Dictionary<string, object>
            {
                { "post.logout.redirect.uris", "https://localhost:44335/signout-callback-oidc" }
            };

            await _keycloakClient.CreateClientAsync(_keycloakOptions.RealmName, publicWebClient);
        }
    }

    private async Task UpdateAdminUserAsync()
    {
        var users = await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: "admin");
        var adminUser = users.FirstOrDefault();
        if (adminUser == null)
        {
            throw new Exception(
                "Keycloak admin user is not provided, check if KEYCLOAK_ADMIN environment variable is passed properly.");
        }

        if (string.IsNullOrEmpty(adminUser.Email))
        {
            adminUser.Email = "admin@abp.io";
            adminUser.FirstName = "admin";
            adminUser.EmailVerified = true;

            await _keycloakClient.UpdateUserAsync(_keycloakOptions.RealmName, adminUser.Id, adminUser);
        }
    }
}