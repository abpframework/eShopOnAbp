using System.Text.Json.Serialization;

namespace EShopOnAbp.DbMigrator.Keycloak.Models;

public class AccessTokenResult
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")] public int Expiration { get; set; }
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiration { get; set; }

    [JsonPropertyName("token_type")] public string TokenType { get; set; }
    [JsonPropertyName("scope")] public string Scope { get; set; }
}