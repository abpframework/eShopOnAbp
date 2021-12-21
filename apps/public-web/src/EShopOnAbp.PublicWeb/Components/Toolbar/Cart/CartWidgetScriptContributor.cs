using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.Modularity;

namespace EShopOnAbp.PublicWeb.Components.Toolbar.Cart;

[DependsOn(typeof(SignalRBrowserScriptContributor))]
public class CartWidgetScriptContributor: BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/components/cart/cart-widget.js");
    }
}