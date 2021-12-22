(function () {
    //set anonymous user
    let anonymousId = abp.utils.getCookieValue("eshop_anonymousId");
    if (anonymousId != null) {
        console.info("Anonymous user set:" + anonymousId);
    }

    var widgetManager = new abp.WidgetManager({
        wrapper: $($('.abp-widget-wrapper[data-widget-name="CartWidget"]')[0])
    });

    $(function () {

        $('.product-list-item').click(function () {
            var $this = $(this);
            var productId = $this.attr('data-product-id');
            eShopOnAbp.basketService.basket.addProduct({
                productId: productId,
                anonymousId: anonymousId,
            }).then(function () {
                widgetManager.refresh();
                abp.notify.success("Added product to your basket.", "Successfully added");
            });
        });
    });
})();