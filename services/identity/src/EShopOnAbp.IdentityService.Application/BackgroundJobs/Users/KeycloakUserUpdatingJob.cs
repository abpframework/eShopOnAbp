using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.Keycloak.Service;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace EShopOnAbp.IdentityService.BackgroundJobs.Users;

public class KeycloakUserUpdatingJob : AsyncBackgroundJob<IdentityUserUpdatingArgs>, ITransientDependency
{
    private readonly IKeycloakService _keycloakService;
    private readonly ILogger<KeycloakUserCreationJob> _logger;
    private readonly IObjectMapper _objectMapper;

    public KeycloakUserUpdatingJob(IKeycloakService keycloakService, ILogger<KeycloakUserCreationJob> logger,
        IObjectMapper objectMapper)
    {
        _keycloakService = keycloakService;
        _logger = logger;
        _objectMapper = objectMapper;
    }

    public override async Task ExecuteAsync(IdentityUserUpdatingArgs args)
    {
        try
        {
            var keycloakUser = (await _keycloakService.GetUsersAsync())
                .FirstOrDefault(q => q.UserName == args.OldUserName);
            if (keycloakUser == null)
            {
                _logger.LogError($"Keycloak user could not be found to update! Username:{args.OldUserName}");
                throw new UserFriendlyException($"Keycloak user with the username:{args.OldUserName} could not be found!");
            }

            IEnumerable<IdentityUserUpdatingArgs.FieldChange> differentFields = args.GetDifferentFields().ToList();
            foreach (var fieldChange in differentFields)
            {
                if (fieldChange.FieldName == "Email")
                    keycloakUser.Email = fieldChange.NewValue.ToString();
                if (fieldChange.FieldName == "UserName")    // Username update is not working - not updating in keycloak
                    keycloakUser.UserName = fieldChange.NewValue.ToString();
                if (fieldChange.FieldName == "Name")
                    keycloakUser.FirstName = fieldChange.NewValue.ToString();
                if (fieldChange.FieldName == "Surname")
                    keycloakUser.LastName = fieldChange.NewValue.ToString();
                if (fieldChange.FieldName == "IsActive")
                    keycloakUser.Enabled = (bool)fieldChange.NewValue;
                if (fieldChange.FieldName == "RoleNames")
                    keycloakUser.RealmRoles = (string[])fieldChange.NewValue;
            }

            if (differentFields.Count() != 0)
            {
                var mappedUser = _objectMapper.Map<CachedKeycloakUser, User>(keycloakUser);

                var result = await _keycloakService.UpdateUserAsync(
                    keycloakUser.Id,
                    mappedUser
                );

                // User roles are not being updated - Updating manually
                if (differentFields.FirstOrDefault(q => q.FieldName == "RoleNames") != null)
                {
                    var oldRoles = (await _keycloakService.GetRolesAsync())
                        .Where(q => args.OldRoleNames.Contains(q.Name))
                        .ToList();
                    var newRoles = (await _keycloakService.GetRolesAsync())
                        .Where(q => args.RoleNames.Contains(q.Name))
                        .ToList();
                    if (oldRoles.Count > 0)
                    {
                        await _keycloakService.RemoveRealmRolesFromUserAsync(keycloakUser.Id,
                            _objectMapper.Map<List<CachedKeycloakRole>, List<Role>>(oldRoles));
                    }

                    if (newRoles.Count > 0)
                    {
                        await _keycloakService.AddRealmRolesToUserAsync(keycloakUser.Id,
                            _objectMapper.Map<List<CachedKeycloakRole>, List<Role>>(newRoles));
                    }
                }

                if (result)
                {
                    _logger.LogInformation($"Keycloak user with the username:{args.UserName} has been updated.");
                }
            }

            if (!args.Password.IsNullOrEmpty() && keycloakUser != null)
            {
                await _keycloakService.SetNewPassword(keycloakUser.UserName, args.Password);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Keycloak user updating failed! Username:{args.UserName}");
            throw new UserFriendlyException($"Keycloak user updating failed! Username:{args.UserName}",
                innerException: e);
        }
    }
}

public class IdentityUserUpdatingArgs
{
    public string Email { get; init; }
    public string OldEmail { get; init; }
    public string UserName { get; init; }
    public string OldUserName { get; init; }
    public string Name { get; init; }
    public string OldName { get; init; }
    public string Surname { get; init; }
    public string OldSurname { get; init; }
    public bool IsActive { get; init; }
    public bool OldIsActive { get; init; }
    public string[] RoleNames { get; init; }
    public string[] OldRoleNames { get; init; }
    public string Password { get; init; }

    public IEnumerable<FieldChange> GetDifferentFields()
    {
        List<FieldChange> fieldChanges = new List<FieldChange>();

        if ((Email, OldEmail) is not (null, null) && Email != OldEmail)
            fieldChanges.Add(new FieldChange { FieldName = nameof(Email), NewValue = Email, OldValue = OldEmail });

        if ((UserName, OldUserName) is not (null, null) && UserName != OldUserName)
            fieldChanges.Add(new FieldChange
                { FieldName = nameof(UserName), NewValue = UserName, OldValue = OldUserName });

        if ((Name, OldName) is not (null, null) && Name != OldName)
            fieldChanges.Add(new FieldChange { FieldName = nameof(Name), NewValue = Name, OldValue = OldName });

        if ((Surname, OldSurname) is not (null, null) && Surname != OldSurname)
            fieldChanges.Add(new FieldChange
                { FieldName = nameof(Surname), NewValue = Surname, OldValue = OldSurname });

        if (IsActive != OldIsActive)
            fieldChanges.Add(new FieldChange
                { FieldName = nameof(IsActive), NewValue = IsActive, OldValue = OldIsActive });

        if (!Enumerable.SequenceEqual(RoleNames ?? Enumerable.Empty<string>(),
                OldRoleNames ?? Enumerable.Empty<string>()))
            fieldChanges.Add(new FieldChange
                { FieldName = nameof(RoleNames), NewValue = RoleNames, OldValue = OldRoleNames });

        return fieldChanges;
    }

    public class FieldChange
    {
        public string FieldName { get; set; }
        public object NewValue { get; set; }
        public object OldValue { get; set; }
    }
}