using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.PublicWeb.Pages;

public class MyOrdersModel : AbpPageModel
{
    public string OrderFilter { get; set; }

    public async Task OnGet(string filter)
    {
        OrderFilter = filter;
    }
}