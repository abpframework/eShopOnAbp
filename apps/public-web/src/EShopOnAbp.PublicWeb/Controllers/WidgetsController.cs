using EShopOnAbp.PublicWeb.Components.Basket;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.PublicWeb.Controllers;

[Route("Widgets")]
public class WidgetsController : AbpController
{
    [HttpGet]
    [Route("Basket")]
    public IActionResult Counters()
    {
        return ViewComponent(typeof(BasketWidgetViewComponent));
    }
}