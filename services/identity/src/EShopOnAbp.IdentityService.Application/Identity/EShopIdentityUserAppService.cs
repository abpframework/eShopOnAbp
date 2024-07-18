using System;
using System.Linq;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.BackgroundJobs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace EShopOnAbp.IdentityService.Identity;

[ExposeServices(typeof(IdentityUserAppService), typeof(IIdentityUserAppService))]
public class EShopIdentityUserAppService : IdentityUserAppService
{
    private readonly IIdentityUserRepository _userRepository;
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IIdentityRoleRepository _roleRepository;

    public EShopIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        IPermissionChecker permissionChecker,
        IBackgroundJobManager backgroundJobManager) : base(userManager,
        userRepository,
        roleRepository,
        identityOptions,
        permissionChecker)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _backgroundJobManager = backgroundJobManager;
    }

    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        var createdUser = await base.CreateAsync(input);
        await _backgroundJobManager.EnqueueAsync(new IdentityUserCreationArgs(input));

        return createdUser;
    }

    public override async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        var existingUser = await _userRepository.GetAsync(id);
        // Disabling username updating. Keycloak service is unavailable to update the username field!
        if (input.UserName != existingUser.UserName)
        {
            input.UserName = existingUser.UserName;
        }
        var args = await CreateIdentityUserUpdatingArgsAsync(existingUser, input);
        var updatedUser = await base.UpdateAsync(id, input);
        await _backgroundJobManager.EnqueueAsync(args);

        return updatedUser;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.FindAsync(id);
        await base.DeleteAsync(id);
        if (user != null)
        {
            await _backgroundJobManager.EnqueueAsync(new IdentityUserDeletionArgs(user.UserName));
        }
    }

    private async Task<IdentityUserUpdatingArgs> CreateIdentityUserUpdatingArgsAsync(IdentityUser existingUser,
        IdentityUserUpdateDto input)
    {
        var userRoles = existingUser.Roles.Select(q => q.RoleId).ToList();
        var roles = await _roleRepository.GetListAsync();

        var args = new IdentityUserUpdatingArgs
        {
            Email = input.Email,
            OldEmail = existingUser.Email,
            UserName = input.UserName,
            OldUserName = existingUser.UserName,
            Name = input.Name,
            OldName = existingUser.Name,
            Surname = input.Surname,
            OldSurname = existingUser.Surname,
            IsActive = input.IsActive,
            OldIsActive = existingUser.IsActive,
            RoleNames = input.RoleNames,
            OldRoleNames = roles.Where(q => userRoles.Contains(q.Id)).Select(q => q.Name).ToArray(),
            Password = input.Password
        };

        return args;
    }
}