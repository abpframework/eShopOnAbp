using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak.Service;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs.Users;

public class KeycloakUserDeletionJob : AsyncBackgroundJob<IdentityUserDeletionArgs>, ITransientDependency
{
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger _logger;

    public KeycloakUserDeletionJob(IKeycloakService keycloakService,
        ILogger<KeycloakUserCreationJob> logger)
    {
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public override async Task ExecuteAsync(IdentityUserDeletionArgs args)
    {
        try
        {
            var keycloakUser = (await _keycloakService.GetUsersAsync())
                .FirstOrDefault(q => q.UserName == args.UserName);
            if (keycloakUser == null)
            {
                _logger.LogError($"Keycloak user could not be found to delete! Username:{args.UserName}");
                throw new UserFriendlyException($"Keycloak user with the username:{args.UserName} could not be found!");
            }

            var result = await _keycloakService.DeleteUserAsync(keycloakUser.Id);
            if (result)
            {
                _logger.LogInformation($"Keycloak user with the username:{args.UserName} has been deleted.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Keycloak user deletion failed! Username:{args.UserName}");
        }
    }
}

public class IdentityUserDeletionArgs
{
    public string UserName { get; init; }

    public IdentityUserDeletionArgs()
    {
    }

    public IdentityUserDeletionArgs(string userName)
    {
        UserName = userName;
    }
}