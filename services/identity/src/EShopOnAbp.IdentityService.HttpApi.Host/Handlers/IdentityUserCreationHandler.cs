using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace EShopOnAbp.IdentityService.Handlers;

public class IdentityUserCreationHandler : IDistributedEventHandler<EntityCreatedEto<UserEto>>, ITransientDependency
{
    private readonly IdentityUserManager _identityUserManager;

    public IdentityUserCreationHandler(IdentityUserManager identityUserManager)
    {
        _identityUserManager = identityUserManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
    {
        if (eventData.Entity.Email != IdentityServiceDbProperties.DefaultAdminEmailAddress)
        {
            var identityUser = await _identityUserManager.FindByIdAsync(eventData.Entity.Id.ToString());
            await _identityUserManager.AddToRoleAsync(identityUser, IdentityServiceDbProperties.CustomerRoleName);
        }
    }
}