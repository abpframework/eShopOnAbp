using System.Collections.Generic;
using System.Threading.Tasks;
using EShopOnAbp.OrderingService.Orders;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages;

public class MyOrdersModel : AbpPageModel
{
    private readonly IOrderAppService _orderAppService;
    public List<OrderDto> MyOrders { get; set; }

    public MyOrdersModel(IOrderAppService orderAppService)
    {
        _orderAppService = orderAppService;
    }

    public async Task OnGet()
    {
        MyOrders = await _orderAppService.GetMyOrdersAsync(new GetMyOrdersInput());
    }
}