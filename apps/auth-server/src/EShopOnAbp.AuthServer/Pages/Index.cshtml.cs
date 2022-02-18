using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EShopOnAbp.AuthServer.Pages
{
    public class IndexModel : AbpPageModel
    {
        public ActionResult OnGet()
        {
            var ipaddr = CurrentUser.FindClaim("ipaddr");

            if (!CurrentUser.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
