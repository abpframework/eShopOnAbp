using System;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs;

public class IdentityUserDeletionJob : AsyncBackgroundJob<IdentityUserDeletionArgs>, ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;
    private readonly ILogger<KeycloakUserCreationJob> _logger;

    public IdentityUserDeletionJob(IOptions<KeycloakClientOptions> keycloakOptions,
        ILogger<KeycloakUserCreationJob> logger)
    {
        _logger = logger;
        _keycloakOptions = keycloakOptions.Value;

        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public override async Task ExecuteAsync(IdentityUserDeletionArgs args)
    {
        try
        {
            var keycloakUser = (await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: args.UserName))
                .First();
            if (keycloakUser == null)
            {
                _logger.LogError($"Keycloak user could not be found to delete! Username:{args.UserName}");
                throw new UserFriendlyException($"Keycloak user with the username:{args.UserName} could not be found!");
            }

            var result = await _keycloakClient.DeleteUserAsync(_keycloakOptions.RealmName, keycloakUser.Id);
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
    public string UserName { get; set; }
}