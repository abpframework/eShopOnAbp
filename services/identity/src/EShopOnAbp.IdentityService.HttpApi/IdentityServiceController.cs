using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EShopOnAbp.IdentityService;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserLookupController), IncludeSelf = true)]
public class IdentityServiceController : IdentityUserLookupController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly IPermissionChecker _permissionChecker;
    private readonly IAuthorizationService _authorizationService;
    public IdentityServiceController(
        IIdentityUserLookupAppService lookupAppService,
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService,
        IPermissionDefinitionManager permissionDefinitionManager, IPermissionChecker permissionChecker) : base(lookupAppService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
        _permissionDefinitionManager = permissionDefinitionManager;
        _permissionChecker = permissionChecker;
    }

    public override async Task<UserData> FindByIdAsync(Guid id)
    {
        //TODO : Why has been called these methods and not used
        var permissions = await _permissionDefinitionManager.GetPermissionsAsync();

        var isGranted = await _permissionChecker.IsGrantedAsync(IdentityPermissions.UserLookup.Default);

        var result = await _authorizationService.IsGrantedAnyAsync("AbpIdentity.UserLookup");
        var httpContext = _httpContextAccessor.HttpContext;
        return await base.FindByIdAsync(id);
    }
}