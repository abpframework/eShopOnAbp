using System;
using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.ServiceProviders
{
    public class UserBasketProvider : ITransientDependency
    {
        protected HttpContext HttpContext => httpContextAccessor.HttpContext;

        private readonly IHttpContextAccessor httpContextAccessor;
        protected readonly ILogger<UserBasketProvider> logger;
        protected readonly IBasketAppService basketAppService;
        protected readonly ICurrentUser currentUser;

        public UserBasketProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserBasketProvider> logger,
            IBasketAppService basketAppService,
            ICurrentUser currentUser)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.basketAppService = basketAppService;
            this.currentUser = currentUser;
        }

        public virtual async Task<BasketDto> GetBasketAsync()
        {
            try
            {
                // Get anonymous user id from cookie
                HttpContext.Request.Cookies.TryGetValue(EShopConstants.AnonymousUserClaimName,
                    out string anonymousUserId);

                if (!currentUser.IsAuthenticated)
                {
                    logger.LogInformation($"Getting basket for anonymous user id:{anonymousUserId}.");
                    return await basketAppService.GetByAnonymousUserIdAsync(Guid.Parse(anonymousUserId));
                }

                //TODO: Merge with anonymously stored cart if exist
                var userClaimValue = currentUser.FindClaimValue(EShopConstants.AnonymousUserClaimName);

                // Fall-back for having trouble when setting claim on user login 
                if (string.IsNullOrEmpty(userClaimValue))
                {
                    return await basketAppService.GetAsync();
                }

                return await basketAppService.MergeBasketsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}