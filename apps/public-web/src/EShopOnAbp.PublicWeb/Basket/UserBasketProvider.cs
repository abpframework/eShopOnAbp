using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Authentication;
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
                var access_token = await HttpContext.GetTokenAsync("access_token");
                logger.LogInformation($"ACCESS_TOKEN:{access_token}");
                // Get anonymous user id from cookie
                HttpContext.Request.Cookies.TryGetValue(EShopConstants.AnonymousUserClaimName,
                    out string anonymousUserId);
                logger.LogInformation($"========= Anonymous User Id from Cookie:{anonymousUserId} ========= ");

                // Generate new id for anonymous user
                if (string.IsNullOrEmpty(anonymousUserId))
                {
                    anonymousUserId = guidGenerator.Create().ToString();
                    HttpContext.Response.Cookies.Append(EShopConstants.AnonymousUserClaimName, anonymousUserId,
                        new CookieOptions
                        {
                            SameSite = SameSiteMode.Lax
                        });
                    logger.LogInformation(
                        $"========= Generated new User Id:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
                }

                if (!currentUser.IsAuthenticated)
                {
                    logger.LogInformation(
                        $"========= Get Basket for Anonymous UserId:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
                    return await basketAppService.GetByAnonymousUserIdAsync(Guid.Parse(anonymousUserId));
                }

                //TODO: Merge with anonymously stored cart if exist
                var userClaimValue = currentUser.FindClaimValue(EShopConstants.AnonymousUserClaimName);
                foreach (var claim in currentUser.GetAllClaims())
                {
                    logger.LogInformation($"Claim Type:{claim.Type}-Value:{claim.Value}");
                }

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