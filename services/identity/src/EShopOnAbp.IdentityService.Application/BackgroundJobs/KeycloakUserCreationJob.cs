using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace EShopOnAbp.IdentityService.BackgroundJobs;

public class KeycloakUserCreationJob : AsyncBackgroundJob<IdentityUserCreationArgs>, ITransientDependency
{
    private readonly KeycloakClient _keycloakClient;
    private readonly KeycloakClientOptions _keycloakOptions;
    private readonly ILogger<KeycloakUserCreationJob> _logger;

    public KeycloakUserCreationJob(IOptions<KeycloakClientOptions> keycloakOptions,
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

    public override async Task ExecuteAsync(IdentityUserCreationArgs args)
    {
        var keycloakUser = new User
        {
            Email = args.Email,
            UserName = args.UserName,
            FirstName = args.Name,
            LastName = args.Surname,
            Enabled = true,
            Credentials = new List<Credentials>()
            {
                new() { Type = "password", Value = args.Password }
            }
        };

        try
        {
            var result = await _keycloakClient.CreateUserAsync(_keycloakOptions.RealmName, keycloakUser);
            if (result)
            {
                _logger.LogInformation($"Keycloak user with the username:{args.UserName} has been created.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Keycloak user creation with the Username:{args.UserName} has been failed!");
            throw;
        }
    }
}

public class IdentityUserCreationArgs
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
}