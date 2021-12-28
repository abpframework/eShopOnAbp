(function () {
    abp.widgets.PaymentWidget = function ($wrapper) {
        var widgetManager = $wrapper.data('abp-widget-manager');

        var init = function (filters) {
            $wrapper
                .find('.address-list .card')
                .click(function () {
                    var $this = $(this);
                    var addressId = $this.attr('data-address-id');
                    abp.utils.setCookieValue("selected-address", addressId);
                    debugger;
                    $this.parents(".address-list").find('.card').removeClass("is-selected");
                    $this.addClass("is-selected");
                });
        };

        return {
            init: init
        };
    };
})();