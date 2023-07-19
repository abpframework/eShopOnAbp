using System;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs;

public class KeycloakUserUpdatingJob : AsyncBackgroundJob<IdentityUserUpdatingArgs>, ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;
    private readonly ILogger<KeycloakUserCreationJob> _logger;

    public KeycloakUserUpdatingJob(IOptions<KeycloakClientOptions> keycloakOptions, ILogger<KeycloakUserCreationJob> logger)
    {
        _logger = logger;
        _keycloakOptions = keycloakOptions.Value;

        _keycloakClient = new KeycloakClient(
            _keycloakOptions.Url,
            _keycloakOptions.AdminUserName,
            _keycloakOptions.AdminPassword
        );
    }

    public override async Task ExecuteAsync(IdentityUserUpdatingArgs args)
    {
        try
        {
            var keycloakUser = (await _keycloakClient.GetUsersAsync(_keycloakOptions.RealmName, username: args.UserName))
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
            keycloakUser.EmailVerified = args.EmailConfirmed;

            var result = await _keycloakClient.UpdateUserAsync(_keycloakOptions.RealmName, keycloakUser.Id, keycloakUser);
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
}