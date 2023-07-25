using System;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.BackgroundJobs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace EShopOnAbp.IdentityService.Identity;

[ExposeServices(typeof(IdentityUserAppService), typeof(IIdentityUserAppService))]
public class EShopIdentityUserAppService : IdentityUserAppService
{
    private readonly IdentityUserManager _userManager;
    private readonly IBackgroundJobManager _backgroundJobManager;

    public EShopIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        IBackgroundJobManager backgroundJobManager) : base(userManager,
        userRepository,
        roleRepository,
        identityOptions)
    {
        _userManager = userManager;
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
        var updatedUser = await base.UpdateAsync(id, input);
        await _backgroundJobManager.EnqueueAsync(new IdentityUserUpdatingArgs
        {
            Email = updatedUser.Email,
            UserName = updatedUser.UserName,
            Name = updatedUser.Name,
            Surname = updatedUser.Surname,
            EmailConfirmed = updatedUser.EmailConfirmed,
            IsActive = input.IsActive,
            RoleNames = input.RoleNames
        });

        return updatedUser;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        await base.DeleteAsync(id);
        if (user != null)
        {
            await _backgroundJobManager.EnqueueAsync(new IdentityUserDeletionArgs(user.UserName));
        }
    }
}