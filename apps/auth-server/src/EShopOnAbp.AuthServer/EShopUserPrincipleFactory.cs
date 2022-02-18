using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EShopOnAbp.AuthServer;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpUserClaimsPrincipalFactory))]
public class EShopUserPrincipleFactory: AbpUserClaimsPrincipalFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EShopUserPrincipleFactory(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        IHttpContextAccessor httpContextAccessor) : base(userManager,
        roleManager,
        options,
        currentPrincipalAccessor,
        abpClaimsPrincipalFactory)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = principal.Identities.First();
        if (_httpContextAccessor.HttpContext != null)
        {
            var auth = await _httpContextAccessor.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (auth.Succeeded)
            {
                var ipaddr =  auth?.Principal?.FindFirst("ipaddr");
                if (ipaddr != null)
                {
                    identity?.AddIfNotContains(ipaddr);
                }
            }
        }

        return principal;
    }
}
