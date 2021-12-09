using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using EShopOnAbp.BasketService;

namespace EShopOnAbp.PublicWeb.Components.Basket;

[Widget]
public class BasketWidgetViewComponent : AbpViewComponent
{
    private readonly IBasketAppService _basketAppService;

    public BasketWidgetViewComponent(IBasketAppService basketAppService)
    {
        _basketAppService = basketAppService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basketDto = await _basketAppService.GetAsync();
        return View("~/Components/Basket/Default.cshtml", basketDto);
    }
}