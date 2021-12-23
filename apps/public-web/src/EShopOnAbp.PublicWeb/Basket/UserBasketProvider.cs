using Castle.Core.Logging;
using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                HttpContext.Request.Cookies.TryGetValue(CookieConstants.AnonymousUserCookieName,
                    out string anonymousUserId);
                logger.LogInformation($"========= Anonymous User Id from Cookie:{anonymousUserId} ========= ");

                // Generate new id for anonymous user
                if (string.IsNullOrEmpty(anonymousUserId))
                {
                    anonymousUserId = guidGenerator.Create().ToString();
                    HttpContext.Response.Cookies.Append(CookieConstants.AnonymousUserCookieName, anonymousUserId);
                    logger.LogInformation($"========= Generated new User Id:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
                }

                if (!currentUser.IsAuthenticated)
                {
                    logger.LogInformation(
                        $"========= Get Basket for Anonymous UserId:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
                    return await basketAppService.GetByAnonymousUserIdAsync(Guid.Parse(anonymousUserId));
                }
                else
                {
                    //TODO: Merge with anonymously stored cart if exist 
                    return await basketAppService.GetAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
