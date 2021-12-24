using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Basket
{
    public class UserBasketProvider : ITransientDependency
    {
        protected HttpContext HttpContext => httpContextAccessor.HttpContext;

        private readonly IHttpContextAccessor httpContextAccessor;
        protected readonly ILogger<UserBasketProvider> logger;
        protected readonly IGuidGenerator guidGenerator;
        protected readonly IBasketAppService basketAppService;
        protected readonly ICurrentUser currentUser;

        public UserBasketProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserBasketProvider> logger,
            IGuidGenerator guidGenerator,
            IBasketAppService basketAppService,
            ICurrentUser currentUser)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.guidGenerator = guidGenerator;
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
                    logger.LogInformation(
                        $"========= Get Basket for Anonymous UserId:{anonymousUserId} =========  ====== ");
                    return await basketAppService.GetByAnonymousUserIdAsync(Guid.Parse(anonymousUserId));
                }

                //TODO: Merge with anonymously stored cart if exist
                var userClaimValue = currentUser.FindClaimValue(EShopConstants.AnonymousUserClaimName);

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