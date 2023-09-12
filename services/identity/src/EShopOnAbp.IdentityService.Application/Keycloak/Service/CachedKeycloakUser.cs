using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Caching;

namespace EShopOnAbp.IdentityService.Keycloak.Service;

[CacheName("KeycloakUser")]
public class CachedKeycloakUser
{
    public string Id { get; set; }
    public long CreatedTimestamp { get; set; }
    public string UserName { get; set; }
    public bool? Enabled { get; set; }
    public bool? Totp { get; set; }
    public bool? EmailVerified { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Collection<string> DisableableCredentialTypes { get; set; }
    public Collection<string> RequiredActions { get; set; }
    public int? NotBefore { get; set; }
    public Dictionary<string, IEnumerable<string>> Attributes { get; set; }
    public IDictionary<string, object> ClientRoles { get; set; }
    public string FederationLink { get; set; }
    public IEnumerable<string> Groups { get; set; }
    public string Origin { get; set; }
    public string[] RealmRoles { get; set; }
    public string Self { get; set; }
    public string ServiceAccountClientId { get; set; }
    public CachedUserAccess Access { get; set; }
    public IEnumerable<CachedUserConsent> ClientConsents { get; set; }
    public IEnumerable<CachedCredentials> Credentials { get; set; }
    public IEnumerable<CachedFederatedIdentity> FederatedIdentities { get; set; }
}

public class CachedUserConsent
{
    public string ClientId { get; set; }
    public IEnumerable<string> GrantedClientScopes { get; set; }
    public long? CreatedDate { get; set; }
    public long? LastUpdatedDate { get; set; }
}

public class CachedUserAccess
{
    public bool? ManageGroupMembership { get; set; }
    public bool? View { get; set; }
    public bool? MapRoles { get; set; }
    public bool? Impersonate { get; set; }
    public bool? Manage { get; set; }
}

public class CachedCredentials
{
    public string Algorithm { get; set; }
    public IDictionary<string, string> Config { get; set; }
    public int? Counter { get; set; }
    public long? CreatedDate { get; set; }
    public string Device { get; set; }
    public int? Digits { get; set; }
    public int? HashIterations { get; set; }
    public string HashSaltedValue { get; set; }
    public int? Period { get; set; }
    public string Salt { get; set; }
    public bool? Temporary { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}

public class CachedFederatedIdentity
{
    public string IdentityProvider { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
}