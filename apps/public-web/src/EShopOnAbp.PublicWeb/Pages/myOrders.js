(function () {
    const anonymousId = abp.utils.getCookieValue("anonymous_id");
    $(function () {
        $('.buy-again').click(function () {
            const $this = $(this);
            const productId = $this.attr('data-product-id');
            eShopOnAbp.basketService.basket.addProduct({
                productId: productId,
                anonymousId: anonymousId,
            }).then(function () {
                $('.abp-widget-wrapper[data-widget-name="CartWidget"]')
                    .data('abp-widget-manager')
                    .refresh();

                abp.notify.success("Added product to your basket.", "Successfully added");
            });
        });
    });
})();