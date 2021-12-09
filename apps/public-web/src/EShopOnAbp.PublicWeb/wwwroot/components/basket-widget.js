(function (){
    abp.widgets.BasketWidget = function ($wrapper) {
        var widgetManager = $wrapper.data('abp-widget-manager');

        var init = function (filters) {
            $wrapper
                .find('.basket-item-remove')
                .click(function(){
                    var $this = $(this);
                    var productId = $this.parent('.basket-list-item').attr('data-product-id');
                    eShopOnAbp.basketService.basket.removeProduct({
                        productId: productId
                    }).then(function(){
                        widgetManager.refresh();
                        abp.notify.info("Removed the product from your basket.", "Removed basket item");
                    });
                });
        };

        return {
            init: init
        };
    };

})();