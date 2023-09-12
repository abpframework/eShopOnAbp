// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using EShopOnAbp.IdentityService.Keycloak;
// using Microsoft.Extensions.Logging;
// using Volo.Abp.DependencyInjection;
// using Volo.Abp.Domain.Entities.Events.Distributed;
// using Volo.Abp.EventBus.Distributed;
// using Volo.Abp.Identity;
//
// namespace EShopOnAbp.IdentityService.EventHandlers.Roles;
//
// public class KeycloakRolesEventHandler : IDistributedEventHandler<EntityCreatedEto<IdentityRoleEto>>,
//     ITransientDependency
// {
//     private readonly KeycloakService _keycloakService;
//     private readonly ILogger<KeycloakRolesEventHandler> _logger;
//
//     public KeycloakRolesEventHandler(KeycloakService keycloakService, ILogger<KeycloakRolesEventHandler> logger)
//     {
//         _keycloakService = keycloakService;
//         _logger = logger;
//     }
//
//     public async Task HandleEventAsync(EntityCreatedEto<IdentityRoleEto> eventData)
//     {
//         try
//         {
//             var existingRole = (await _keycloakService.GetRolesAsync()).FirstOrDefault(q => q.Name == eventData.Entity.Name);
//             if (existingRole != null)
//             {
//                 return;
//             }
//
//             var isSuccess = await _keycloakService.CreateRoleAsync(eventData.Entity.Name);
//             if (isSuccess)
//             {
//                 _logger.LogInformation($"Role created:{eventData.Entity.Name}");
//             }
//         }
//         catch (Exception e)
//         {
//             _logger.LogError($"Keycloak role creation with the name:{eventData.Entity.Name} failed!");
//             throw;
//         }
//     }
// }