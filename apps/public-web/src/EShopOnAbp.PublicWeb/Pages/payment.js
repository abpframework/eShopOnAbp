(function () {
    let form = document.querySelector("form");
    
    form.onsubmit = function (e) {
        let addressInput = document.createElement('input');
        addressInput.setAttribute('name', "model.SelectedAddressId");//set the param name
        addressInput.setAttribute('value', document.querySelector(".address-list .card.is-selected").getAttribute("data-address-id"));
        addressInput.setAttribute('type', "hidden");
        form.appendChild(addressInput);

        let paymentInput = document.createElement('input');
        paymentInput.setAttribute('name', "model.SelectedPaymentType");
        paymentInput.setAttribute('value', document.querySelector(".payment-list .card.is-selected").getAttribute("data-payment-type"));
        paymentInput.setAttribute('type', "hidden");
        form.appendChild(paymentInput);
    };
})();