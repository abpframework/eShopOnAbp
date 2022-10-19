using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
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
        await CreateClientsAsync();
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