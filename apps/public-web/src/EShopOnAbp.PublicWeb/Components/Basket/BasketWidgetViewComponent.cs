using System.Collections;
using System.Collections.Generic;
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

    public BasketWidgetViewComponent()
    {
        //_basketAppService = basketAppService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basketModel = new BasketViewModel();
        basketModel.Items.Add(new BasketItemViewModel{ImageName = "asus.jpg", ProductName = "test-one", Count = 1, TotalPrice = 19});
        basketModel.Items.Add(new BasketItemViewModel{ImageName = "lego.jpg", ProductName = "test-two", Count = 2, TotalPrice = 4});
        basketModel.TotalPrice = 123;
        return View("~/Components/Basket/Default.cshtml", basketModel);
    }
}

public class BasketViewModel
{
    public float TotalPrice { get; set; }
    public List<BasketItemViewModel> Items { get; set; }

    public BasketViewModel()
    {
        Items = new List<BasketItemViewModel>();
    }
}

public class BasketItemViewModel
{
    public string ProductName { get; set; }
    public int Count { get; set; }
    public float TotalPrice { get; set; }
    public string ImageName { get; set; }
}