using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace EShopOnAbp.IdentityService.BackgroundJobs.Users;

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
        var keycloakUser = new User
        {
            Email = args.Email,
            UserName = args.UserName,
            FirstName = args.Name,
            LastName = args.Surname,
            Enabled = args.IsActive,
            // EmailVerified = true,
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
    public string Email { get; init; }
    public string UserName { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Password { get; init; }
    public bool IsActive { get; init; }
    public string[] RoleNames { get; init; }

    public IdentityUserCreationArgs()
    {
        
    }

    public IdentityUserCreationArgs(IdentityUserCreateDto input)
    {
        Email = input.Email;
        UserName = input.UserName;
        Name = input.Name;
        Surname = input.Surname;
        Password = input.Password;
        IsActive = input.IsActive;
        RoleNames = input.RoleNames;
    }
}