using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace EShopOnAbp.AuthServer;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpUserClaimsPrincipalFactory))]
public class EShopUserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<EShopUserClaimsPrincipalFactory> _logger;
    private readonly AnonymousUserProvider _anonymousUserProvider;
    protected HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public EShopUserClaimsPrincipalFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        IHttpContextAccessor httpContextAccessor, 
        ILogger<EShopUserClaimsPrincipalFactory> logger, 
        AnonymousUserProvider anonymousUserProvider) : base(userManager,
        roleManager,
        options,
        currentPrincipalAccessor,
        abpClaimsPrincipalFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _anonymousUserProvider = anonymousUserProvider;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        if (HttpContext.Request.Cookies.ContainsKey(EShopConstants.AnonymousUserClaimName))
        {
            HttpContext.Request.Cookies.TryGetValue(EShopConstants.AnonymousUserClaimName, out var anonymousUserId);
            _anonymousUserProvider.AnonymousUserId = anonymousUserId;
        }
        
        principal.Identities.First()
            .AddClaim(new Claim(EShopConstants.AnonymousUserClaimName, _anonymousUserProvider.AnonymousUserId));
        _logger.LogInformation(
            $"Added {EShopConstants.AnonymousUserClaimName} claim of AnonymousUserId from cookies:{_anonymousUserProvider.AnonymousUserId}");

        return principal;
    }
}