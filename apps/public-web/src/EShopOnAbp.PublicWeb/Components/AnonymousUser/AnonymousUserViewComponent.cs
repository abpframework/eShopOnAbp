using System;
using System.Threading.Tasks;
using EShopOnAbp.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Guids;

namespace EShopOnAbp.PublicWeb.Components.AnonymousUser;

[Widget(
    AutoInitialize = true
)]
public class AnonymousUserViewComponent : AbpViewComponent
{
    private readonly ILogger<AnonymousUserViewComponent> _logger;
    private readonly IGuidGenerator _guidGenerator;

    public AnonymousUserViewComponent(ILogger<AnonymousUserViewComponent> logger, IGuidGenerator guidGenerator)
    {
        _logger = logger;
        _guidGenerator = guidGenerator;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Get anonymous user id from cookie
        HttpContext.Request.Cookies.TryGetValue(EShopConstants.AnonymousUserClaimName,
            out string anonymousUserId);
        _logger.LogInformation($"========= Anonymous User Id from Cookie:{anonymousUserId} ========= ");

        // Generate new id for anonymous user
        if (string.IsNullOrEmpty(anonymousUserId))
        {
            anonymousUserId = _guidGenerator.Create().ToString();
            HttpContext.Response.Cookies.Append(EShopConstants.AnonymousUserClaimName, anonymousUserId,
                new CookieOptions
                {
                    SameSite = SameSiteMode.Lax
                });
            _logger.LogInformation(
                $"========= Generated new User Id:{anonymousUserId} ========= APPENDED TO COOKIE ====== ");
        }
        return await Task.FromResult(View("~/Components/AnonymousUser/Default.cshtml"));
    }
}