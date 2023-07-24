using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.Keycloak;

public class KeycloakService : ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;

    public KeycloakService(IOptions<KeycloakClientOptions> keycloakOptions)
    {
        _keycloakOptions = keycloakOptions.Value;

        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public Task<IEnumerable<User>> GetUsersAsync(string search = null, string username = null, string email = null,
        CancellationToken cancellationToken = default)
    {
        return _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, search: search, username: username,
            email: email, cancellationToken: cancellationToken);
    }

    public Task<User> GetUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return _keycloakClient.GetUserAsync(_keycloakOptions.RealmName, userId, cancellationToken: cancellationToken);
    }

    public Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return _keycloakClient.CreateUserAsync(_keycloakOptions.RealmName, user, cancellationToken);
    }

    public Task<bool> UpdateUserAsync(string userId, User user, CancellationToken cancellationToken = default)
    {
        return _keycloakClient.UpdateUserAsync(_keycloakOptions.RealmName, userId, user, cancellationToken);
    }

    public Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return _keycloakClient.DeleteUserAsync(_keycloakOptions.RealmName, userId, cancellationToken);
    }

    public Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return _keycloakClient.GetRolesAsync(_keycloakOptions.RealmName, cancellationToken: cancellationToken);
    }

    public Task<bool> AddRolesToUserAsync(string userId, IEnumerable<Role> roles,
        CancellationToken cancellationToken = default)
    {
        return _keycloakClient.AddRealmRoleMappingsToUserAsync(_keycloakOptions.RealmName, userId, roles,
            cancellationToken);
    }
}