using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Footer
{
    public class FooterComponent : AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/Toolbar/Footer/Default.cshtml");
        }
    }
}
