(function(){
    $(function(){
        $('.product-list-item').click(function(){
            var $this = $(this);
            var productId = $this.attr('data-product-id');
            eShopOnAbp.basketService.basket.addProduct({
                productId: productId
            }).then(function(){
                abp.notify.success("Added product to your basket.", "Successfully added");
            });
        });
    })
})();