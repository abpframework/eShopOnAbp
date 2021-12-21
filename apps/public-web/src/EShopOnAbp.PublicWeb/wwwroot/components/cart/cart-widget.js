(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalr-hubs/basket")
        .build();

    connection.on("BasketProductUpdated", function (data) {
        var widgetManager = $wrapper.data('abp-widget-manager');
        //todo:test
        debugger;
        $('#data-cart-count-id').text(data);
        widgetManager.refresh();
        abp.notify.info('The product "' + data.productName + '" has been changed!', 'Your basket has been updated!');
    });

    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });

})();