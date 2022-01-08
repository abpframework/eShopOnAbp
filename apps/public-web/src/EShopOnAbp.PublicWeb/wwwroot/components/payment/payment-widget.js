(function () {
    abp.widgets.PaymentWidget = function ($wrapper) {
        var widgetManager = $wrapper.data('abp-widget-manager');

        var init = function (filters) {
            $wrapper
                .find('.address-list .card')
                .click(function () {
                    const $this = $(this);
                    // const addressId = $this.attr('data-address-id');
                    $this.parents(".address-list").find('.card').removeClass("is-selected");
                    $this.addClass("is-selected");
                });

            $wrapper
                .find('.payment-list .card')
                .click(el => {
                    const $this = $(el.currentTarget);
                    // const paymentTypeId = $this.attr('data-payment-id');
                    $this.parents(".payment-list").find('.card').removeClass("is-selected");
                    $this.addClass("is-selected");
                });
        };

        return {
            init: init
        };
    };
})();