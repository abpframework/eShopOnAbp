(function(){
    $(function(){
        var basketWidget = new abp.WidgetManager('#BasketArea');

        $('.product-list-item').click(function(){
            var $this = $(this);
            var productId = $this.attr('data-product-id');
            eShopOnAbp.basketService.basket.addProduct({
                productId: productId
            }).then(function(){
                basketWidget.refresh();
                abp.notify.success("Added product to your basket.", "Successfully added");
            });
        });
    });
})();