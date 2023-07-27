// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Keycloak.Net.Models.Roles;
// using Keycloak.Net.Models.Users;
// using Volo.Abp.Caching;
// using Volo.Abp.DependencyInjection;
//
// namespace EShopOnAbp.IdentityService.Keycloak.Service;
//
// [ExposeServices(typeof(CachedKeycloakService))]
// public class CachedKeycloakService : IKeycloakService
// {
//     protected const string UsersCacheKey = "KeycloakUsers";
//     protected const string RolesCacheKey = "KeycloakRoles";
//     private readonly IKeycloakService _keycloakService;
//     private readonly IDistributedCache<List<User>> _keycloakUsersCache;
//     private readonly IDistributedCache<List<Role>> _keycloakRolesCache;
//
//     public CachedKeycloakService(IKeycloakService keycloakService,
//         IDistributedCache<List<User>> keycloakUsersCache,
//         IDistributedCache<List<Role>> keycloakRolesCache)
//     {
//         _keycloakService = keycloakService;
//         _keycloakUsersCache = keycloakUsersCache;
//         _keycloakRolesCache = keycloakRolesCache;
//     }
//
//     public async Task<IEnumerable<User>> GetUsersAsync(string search = null, string username = null,
//         string email = null,
//         CancellationToken cancellationToken = default)
//     {
//         var users = await _keycloakUsersCache.GetAsync(UsersCacheKey, token: cancellationToken);
//         if (users == null)
//         {
//             users = (await _keycloakService.GetUsersAsync(search, username, email, cancellationToken)).ToList();
//             await _keycloakUsersCache.SetAsync(UsersCacheKey, users, token: cancellationToken);
//         }
//
//         return users;
//     }
//
//     public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.CreateUserAsync(user, cancellationToken);
//         if (result)
//         {
//             await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
//
//     public async Task<bool> UpdateUserAsync(string userId, User user, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.UpdateUserAsync(userId, user, cancellationToken);
//         if (result)
//         {
//             await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
//
//     public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.DeleteUserAsync(userId, cancellationToken);
//         if (result)
//         {
//             await _keycloakUsersCache.RemoveAsync(UsersCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
//
//     public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
//     {
//         var roles = await _keycloakRolesCache.GetAsync(RolesCacheKey, token: cancellationToken);
//         if (roles == null)
//         {
//             roles = (await _keycloakService.GetRolesAsync(cancellationToken: cancellationToken)).ToList();
//             await _keycloakRolesCache.SetAsync(RolesCacheKey, roles, token: cancellationToken);
//         }
//
//         return roles;
//     }
//
//     public Task<bool> AddRolesToUserAsync(string userId, IEnumerable<Role> roles,
//         CancellationToken cancellationToken = default)
//     {
//         return _keycloakService.AddRolesToUserAsync(userId, roles, cancellationToken);
//     }
//
//     public async Task<bool> CreateRoleAsync(string name, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.CreateRoleAsync(name, cancellationToken);
//         if (result)
//         {
//             await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
//
//     public async Task<bool> DeleteRoleByIdAsync(string id, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.DeleteRoleByIdAsync(id, cancellationToken);
//         if (result)
//         {
//             await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
//
//     public async Task<bool> UpdateRoleAsync(string id, Role role, CancellationToken cancellationToken = default)
//     {
//         var result = await _keycloakService.UpdateRoleAsync(id, role, cancellationToken);
//         if (result)
//         {
//             await _keycloakRolesCache.RemoveAsync(RolesCacheKey, token: cancellationToken);
//         }
//
//         return result;
//     }
// }