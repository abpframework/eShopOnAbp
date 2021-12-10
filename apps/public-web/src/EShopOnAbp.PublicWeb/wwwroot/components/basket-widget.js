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
            
            $wrapper
                .find('.basket-purchase-button')
                .click(function(){
                    eShopOnAbp.basketService.basket.purchase().then(function(){
                        widgetManager.refresh();
                        abp.notify.info("Thank you for the payment!", "Purchase completed!");
                    });                    
                });
        };

        return {
            init: init
        };
    };

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalr-hubs/basket")
        .build();
    
    connection.on("BasketProductUpdated", function (data) {
        $('.basket-list')
            .closest('.abp-widget-wrapper')
            .each(function(){
                var $wrapper = $(this);
                if ($wrapper.find('[data-product-id=' + data.productId + ']').length){
                    var widgetManager = new abp.WidgetManager({
                        wrapper: $wrapper
                    });
                    widgetManager.refresh();
                    abp.notify.info('The product "' + data.productName + '" has been changed!', 'Your basket has been updated!');
                }
            });
    });

    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });

})();