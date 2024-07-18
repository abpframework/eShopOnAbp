using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace EShopOnAbp.IdentityService.Keycloak.Service;

/*
 * This will be an external service from the Keycloak package
 */
[ExposeServices(typeof(IKeycloakService), typeof(KeycloakService))]
public class KeycloakService : IKeycloakService
{
    protected const string UsersCacheKey = "KeycloakUsers";
    protected const string RolesCacheKey = "KeycloakRoles";
    private readonly IObjectMapper _objectMapper;
    private readonly IDistributedCache<List<CachedKeycloakUser>, string> _keycloakUsersCache;
    private readonly IDistributedCache<List<CachedKeycloakRole>, string> _keycloakRolesCache;

    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;

    public KeycloakService(
        IOptions<KeycloakClientOptions> keycloakOptions,
        IObjectMapper objectMapper,
        IDistributedCache<List<CachedKeycloakUser>, string> keycloakUsersCache,
        IDistributedCache<List<CachedKeycloakRole>, string> keycloakRolesCache)
    {
        _objectMapper = objectMapper;
        _keycloakUsersCache = keycloakUsersCache;
        _keycloakRolesCache = keycloakRolesCache;

        _keycloakOptions = keycloakOptions.Value;
        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public async Task<List<CachedKeycloakUser>> GetUsersAsync(string search = null, string username = null,
        string email = null,
        CancellationToken cancellationToken = default)
    {
        var users = await _keycloakUsersCache.GetAsync(UsersCacheKey, token: cancellationToken);
        if (users == null)
        {
            var result = await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, search: search,
                username: username,
                email: email, cancellationToken: cancellationToken);
            users = _objectMapper.Map<List<User>, List<CachedKeycloakUser>>(result.ToList());
            await _keycloakUsersCache.SetAsync(UsersCacheKey, users, token: cancellationToken);
        }

        return users;
    }

    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.CreateUserAsync(_keycloakOptions.RealmName, user, cancellationToken);
        if (result)
        {
            await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<bool> UpdateUserAsync(string userId, User user, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.UpdateUserAsync(_keycloakOptions.RealmName, userId, user, cancellationToken);
        if (result)
        {
            await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.DeleteUserAsync(_keycloakOptions.RealmName, userId, cancellationToken);
        if (result)
        {
            await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<List<CachedKeycloakRole>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _keycloakRolesCache.GetAsync(RolesCacheKey, token: cancellationToken);
        if (roles == null)
        {
            var result =
                (await _keycloakClient.GetRolesAsync(_keycloakOptions.RealmName, cancellationToken: cancellationToken))
                .ToList();
            roles = _objectMapper.Map<List<Role>, List<CachedKeycloakRole>>(result.ToList());

            await _keycloakRolesCache.SetAsync(RolesCacheKey, roles, token: cancellationToken);
        }

        return roles;
    }

    public Task<bool> AddRealmRolesToUserAsync(string userId, IEnumerable<Role> roles,
        CancellationToken cancellationToken = default)
    {
        return _keycloakClient.AddRealmRoleMappingsToUserAsync(_keycloakOptions.RealmName, userId, roles,
            cancellationToken);
    }

    public Task<bool> RemoveRealmRolesFromUserAsync(string userId, IEnumerable<Role> roles,
        CancellationToken cancellationToken = default)
    {
        return _keycloakClient.DeleteRealmRoleMappingsFromUserAsync(_keycloakOptions.RealmName, userId, roles,
            cancellationToken);
    }

    public async Task<bool> CreateRoleAsync(string name, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.CreateRoleAsync(_keycloakOptions.RealmName, new Role() { Name = name },
            cancellationToken);
        if (result)
        {
            await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<bool> DeleteRoleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.DeleteRoleByIdAsync(_keycloakOptions.RealmName, id, cancellationToken);
        if (result)
        {
            await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<bool> UpdateRoleAsync(string id, Role role, CancellationToken cancellationToken = default)
    {
        var result = await _keycloakClient.UpdateRoleByIdAsync(_keycloakOptions.RealmName, id, role, cancellationToken);
        if (result)
        {
            await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
        }

        return result;
    }

    public async Task<bool> SetNewPassword(string username, string newPassword, CancellationToken cancellationToken = default)
    {
        var users = await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: username, cancellationToken: cancellationToken);

        if (!users.Any()) return false;

        var user = users.First();

        var newCredentials = new Credentials
        {
            Type = "password",
            Value = newPassword,
            Temporary = false
        };

        var isSuccessReset = await _keycloakClient.ResetUserPasswordAsync(
            realm: _keycloakOptions.RealmName,
            userId: user.Id,
            credentials: newCredentials,
            cancellationToken: cancellationToken
        );

        return isSuccessReset;
    }
}