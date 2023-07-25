using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs.Roles;

public class KeycloakRoleUpdatingJob : AsyncBackgroundJob<IdentityRoleUpdatingArgs>, ITransientDependency
{
    private readonly KeycloakService _keycloakService;
    private readonly ILogger<KeycloakRoleUpdatingJob> _logger;

    public KeycloakRoleUpdatingJob(KeycloakService keycloakService, ILogger<KeycloakRoleUpdatingJob> logger)
    {
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public override async Task ExecuteAsync(IdentityRoleUpdatingArgs args)
    {
        try
        {
            var existingRole = (await _keycloakService.GetRolesAsync()).FirstOrDefault(q => q.Name == args.Name);
            if (existingRole == null)
            {
                _logger.LogWarning($"Role with the name:{args.Name} couldn't be found to update!");
                return;
            }

            existingRole.Name = args.Name;

            await _keycloakService.UpdateRoleAsync(existingRole.Id, existingRole);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Could not delete the role with the name:{args.Name} from Keycloak server!");
            throw;
        }
    }
}

public record IdentityRoleUpdatingArgs(string Name);