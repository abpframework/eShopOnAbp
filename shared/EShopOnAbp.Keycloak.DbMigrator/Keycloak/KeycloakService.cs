using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EShopOnAbp.DbMigrator.Keycloak.Models;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.DbMigrator.Keycloak;

public class KeycloakService : ITransientDependency
{
    public const string HttpClientName = "KeycloakServiceHttpClientName";

    // TODO: Option
    public const string BaseUrl = "http://localhost:8080/admin/realms";
    public const string AdminClientId = "admin-cli";
    public const string AdminUserName = "admin";
    public const string AdminPassword = "1q2w3E*";

    private readonly ILogger<KeycloakService> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public KeycloakService(IHttpClientFactory clientFactory, ILogger<KeycloakService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    // public async Task<string> CreateClientAsync(string realm, Client client)
    // {
    //     HttpResponseMessage response = await InternalCreateClientAsync(realm, client).ConfigureAwait(false);
    //
    //     var locationPathAndQuery = response.Headers.Location.PathAndQuery;
    //     var clientId = response.IsSuccessStatusCode
    //         ? locationPathAndQuery.Substring(locationPathAndQuery.LastIndexOf("/", StringComparison.Ordinal) + 1)
    //         : null;
    //     return clientId;
    // }

    private HttpClient CreateKeycloakApiHttpClient(string realm)
    {
        var httpClient = _clientFactory.CreateClient(HttpClientName);
        httpClient.BaseAddress = new Uri(BaseUrl);

        return httpClient;
    }

    private async Task<HttpClient> CreateKeycloakApiHttpClientAsync(string realm, string token = null)
    {
        var httpClient = CreateKeycloakApiHttpClient(realm);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        if (!string.IsNullOrEmpty(token))
        {
            var accessToken = await GetAdminAccessTokenAsync(realm);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{accessToken}");
        }

        return httpClient;
    }

    public async Task<string> GetAdminAccessTokenAsync(string realm)
    {
        var httpClient = _clientFactory.CreateClient(HttpClientName);
        httpClient.BaseAddress = new Uri(BaseUrl);

        var result = string.Empty;

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", AdminClientId),
            new KeyValuePair<string, string>("username", AdminUserName),
            new KeyValuePair<string, string>("password", AdminPassword),
            new KeyValuePair<string, string>("grant_type", "password")
        });

        var httpResponseMessage =
            await httpClient.PostAsync($"/realms/{realm}/protocol/openid-connect/token", formContent);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<AccessTokenResult>();
            result = response?.AccessToken;
        }

        return result;
    }
}