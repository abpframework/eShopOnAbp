using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EShopOnAbp.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Guids;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Cart;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/Cart",
    StyleTypes = new[] {typeof(CartWidgetStyleContributor)},
    ScriptTypes = new[] {typeof(CartWidgetScriptContributor)}
)]
public class CartWidgetViewComponent : AbpViewComponent
{
    private readonly IBasketAppService _basketAppService;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentUser _currentUser;

    protected const string AnonymousUserCookieName = "eshop_anonymousId";

    public CartWidgetViewComponent(
        IBasketAppService basketAppService,
        ICurrentUser currentUser,
        IGuidGenerator guidGenerator)
    {
        _basketAppService = basketAppService;
        _currentUser = currentUser;
        _guidGenerator = guidGenerator;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        BasketDto basketDto = new BasketDto();
        try
        {
            if (!_currentUser.IsAuthenticated)
            {
                var anonymousUserId = _guidGenerator.Create();
                HttpContext.Response.Cookies.Append(AnonymousUserCookieName, anonymousUserId.ToString());
                basketDto = await _basketAppService.GetByAnonymousUserIdAsync(anonymousUserId);
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