using System;
using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Cart;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/Basket",
    StyleTypes = new[] {typeof(CartWidgetStyleContributor)},
    ScriptTypes = new[] {typeof(CartWidgetScriptContributor)}
)]
public class CartWidgetViewComponent : AbpViewComponent
{
    private readonly IBasketAppService _basketAppService;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<CartWidgetViewComponent> _logger;

    public CartWidgetViewComponent(
        IBasketAppService basketAppService,
        ICurrentUser currentUser,
        IGuidGenerator guidGenerator, 
        ILogger<CartWidgetViewComponent> logger)
    {
        _basketAppService = basketAppService;
        _currentUser = currentUser;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        BasketDto basketDto = new BasketDto();
        try
        {
            // Get anonymous user id from cookie
            HttpContext.Request.Cookies.TryGetValue(CookieConstants.AnonymousUserCookieName,
                out string anonymousUserId);
            _logger.LogInformation($"========= Anonymous User Id from Cookie:{anonymousUserId} ========= ");
            
            // Generate new id for anonymous user
            if (string.IsNullOrEmpty(anonymousUserId))
            {
                anonymousUserId = _guidGenerator.Create().ToString();
                HttpContext.Response.Cookies.Append(CookieConstants.AnonymousUserCookieName, anonymousUserId);
                _logger.LogInformation($"========= Generated new User Id:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
            }
            
            if (!_currentUser.IsAuthenticated)
            {
                basketDto = await _basketAppService.GetByAnonymousUserIdAsync(Guid.Parse(anonymousUserId));
                _logger.LogInformation(
                    $"========= Get Basket for Anonymous UserId:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
                _logger.LogInformation($"========= Basket Item Count:{basketDto.Items.Count} ========= ");
            }
            else
            {
                //TODO: Merge with anonymously stored cart if exist 
                basketDto = await _basketAppService.GetAsync();
            }
        }
        catch (Exception e)
        {
            basketDto = null;
            Console.WriteLine(e);
        }

        return View("~/Components/Toolbar/Cart/Default.cshtml", basketDto);
    }
}