using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Keycloak.Net.Models.Users;

namespace EShopOnAbp.IdentityService.BackgroundJobs.User;

public class KeycloakUserCreationJob : AsyncBackgroundJob<IdentityUserCreationArgs>, ITransientDependency
{
    private readonly KeycloakService _keycloakService;
    private readonly ILogger _logger;

    public KeycloakUserCreationJob(KeycloakService keycloakService,
        ILogger<KeycloakUserCreationJob> logger)
    {
        _logger = logger;
        _keycloakService = keycloakService;
    }

    public override async Task ExecuteAsync(IdentityUserCreationArgs args)
    {
        var keycloakUser = new global::Keycloak.Net.Models.Users.User()
        {
            Email = args.Email,
            UserName = args.UserName,
            FirstName = args.Name,
            LastName = args.Surname,
            Enabled = args.IsActive,
            Credentials = new List<Credentials>()
            {
                new() { Type = "password", Value = args.Password }
            }
        };

        try
        {
            var result = await _keycloakService.CreateUserAsync(keycloakUser);
            if (result)
            {
                if (args.RoleNames.Length != 0)
                {
                    await AddRolesToKeycloakUserAsync(keycloakUser.UserName, args.RoleNames);
                }

                _logger.LogInformation($"Keycloak user with the username:{args.UserName} has been created.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Keycloak user creation with the Username:{args.UserName} has been failed!");
            throw;
        }
    }

    private async Task AddRolesToKeycloakUserAsync(string userName, string[] roleNames)
    {
        var user = (await _keycloakService.GetUsersAsync(username: userName)).First();
        var allTheRoles = await _keycloakService.GetRolesAsync();
        var roles = allTheRoles.Where(q => roleNames.Contains(q.Name));

        await _keycloakService.AddRolesToUserAsync(user.Id, roles);
        _logger.LogInformation($"Keycloak roles:{roleNames} has been added to user with the username:{userName}.");
    }
}

public class IdentityUserCreationArgs
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public string[] RoleNames { get; set; }
}