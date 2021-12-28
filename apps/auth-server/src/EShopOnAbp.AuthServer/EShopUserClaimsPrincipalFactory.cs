using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
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
    private readonly IDistributedCache<AnonymousUserItem, Guid> _cache;
    protected HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public EShopUserClaimsPrincipalFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<EShopUserClaimsPrincipalFactory> logger,
        IDistributedCache<AnonymousUserItem, Guid> cache) : base(userManager,
        roleManager,
        options,
        currentPrincipalAccessor,
        abpClaimsPrincipalFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _cache = cache;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        if (HttpContext.Request.Cookies.ContainsKey(EShopConstants.AnonymousUserClaimName))
        {
            HttpContext.Request.Cookies.TryGetValue(EShopConstants.AnonymousUserClaimName, out var anonymousUserId);

            await _cache.SetAsync(user.Id, new AnonymousUserItem {AnonymousUserId = anonymousUserId},
                options: new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(60)
                });
            _logger.LogInformation($"Cached anonymousUserId:{anonymousUserId}.");
        }

        var cachedItem = await _cache.GetAsync(user.Id);
        if (cachedItem == null)
        {
            _logger.LogWarning($"Could not retrieve anonymousUserId from cache.");
            return principal;
        }

        _logger.LogInformation($"Retrieved anonymousUserId:{cachedItem.AnonymousUserId} from cache.");
        principal.Identities.First()
            .AddClaim(new Claim(EShopConstants.AnonymousUserClaimName, cachedItem.AnonymousUserId));

        return principal;
    }
}