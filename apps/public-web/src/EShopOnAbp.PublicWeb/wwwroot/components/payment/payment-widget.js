(function () {
    // Write selected payment type to cookie anyways
    const paymentTypeId = $(".payment-list").find(".is-selected").attr('data-payment-id');
    abp.utils.setCookieValue("selected_payment_id", paymentTypeId);

    abp.widgets.PaymentWidget = function ($wrapper) {
        var widgetManager = $wrapper.data('abp-widget-manager');

        var init = function (filters) {
            $wrapper
                .find('.address-list .card')
                .click(function () {
                    const $this = $(this);
                    $this.parents(".address-list").find('.card').removeClass("is-selected");
                    $this.addClass("is-selected");
                });

            $wrapper
                .find('.payment-list .card')
                .click(el => {
                    const $this = $(el.currentTarget);
                    const paymentTypeId = $this.attr('data-payment-id');
                    abp.utils.setCookieValue("selected_payment_id", paymentTypeId);
                    $this.parents(".payment-list").find('.card').removeClass("is-selected");
                    $this.addClass("is-selected");
                });
        };

        return {
            init: init
        };
    };
})();