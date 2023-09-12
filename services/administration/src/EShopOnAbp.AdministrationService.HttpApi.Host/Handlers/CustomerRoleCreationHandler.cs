using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.AdministrationService.Handlers;

public class CustomerRoleCreationHandler : IDistributedEventHandler<EntityCreatedEto<IdentityRoleEto>>,
    ITransientDependency
{
    private readonly IPermissionManager _permissionManager;
    protected const string CustomerRole = "customer";
    protected const string OrderPermission = "OrderingService.Orders";

    public CustomerRoleCreationHandler(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEto<IdentityRoleEto> eventData)
    {
        // Assign initial permissions for the customer role
        await _permissionManager.SetForRoleAsync(CustomerRole, OrderPermission, true);
    }
}