using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak;
using Keycloak.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs.User;

public class KeycloakUserUpdatingJob : AsyncBackgroundJob<IdentityUserUpdatingArgs>, ITransientDependency
{
    private readonly KeycloakService _keycloakService;
    private readonly ILogger<KeycloakUserCreationJob> _logger;

    public KeycloakUserUpdatingJob(KeycloakService keycloakService, ILogger<KeycloakUserCreationJob> logger)
    {
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public override async Task ExecuteAsync(IdentityUserUpdatingArgs args)
    {
        try
        {
            var keycloakUser = (await _keycloakService.GetUsersAsync( username: args.UserName))
                .First();
            if (keycloakUser == null)
            {
                _logger.LogError($"Keycloak user could not be found to update! Username:{args.UserName}");
                throw new UserFriendlyException($"Keycloak user with the username:{args.UserName} could not be found!");
            }

            keycloakUser.UserName = args.UserName;
            keycloakUser.Email = args.Email;
            keycloakUser.FirstName = args.Name;
            keycloakUser.LastName = args.Surname;
            keycloakUser.Enabled = args.IsActive;
            keycloakUser.EmailVerified = args.EmailConfirmed;

            var result =
                await _keycloakService.UpdateUserAsync( keycloakUser.Id, keycloakUser);
            if (result)
            {
                _logger.LogInformation($"Keycloak user with the username:{args.UserName} has been updated.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Keycloak user updating failed! Username:{args.UserName}");
        }
    }
}

public class IdentityUserUpdatingArgs
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsActive { get; set; }
    public string[] RoleNames { get; set; }
}