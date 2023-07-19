using System;
using System.Threading.Tasks;
using EShopOnAbp.IdentityService.BackgroundJobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace EShopOnAbp.IdentityService;

[ExposeServices(typeof(IdentityUserAppService), typeof(IIdentityUserAppService))]
public class EShopIdentityUserAppService : IdentityUserAppService
{
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IIdentityUserRepository _userRepository;

    public EShopIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions, IBackgroundJobManager backgroundJobManager) : base(userManager,
        userRepository,
        roleRepository,
        identityOptions)
    {
        _backgroundJobManager = backgroundJobManager;
        _userRepository = userRepository;
    }

    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        var createdUser = await base.CreateAsync(input);
        await _backgroundJobManager.EnqueueAsync(new IdentityUserCreationArgs
        {
            Email = createdUser.Email,
            UserName = createdUser.UserName,
            Name = createdUser.Name,
            Surname = createdUser.Surname,
            Password = input.Password
        });

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
            EmailConfirmed = updatedUser.EmailConfirmed
        });

        return updatedUser;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.FindAsync(id);
        await base.DeleteAsync(id);
        
        await _backgroundJobManager.EnqueueAsync(new IdentityUserDeletionArgs
        {
            UserName = user.UserName
        });
    }
}