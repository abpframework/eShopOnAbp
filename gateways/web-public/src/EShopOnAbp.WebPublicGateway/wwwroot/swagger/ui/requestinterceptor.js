const originalFetch = window.fetch;

window.fetch = function (input, init) {
    if (init !== undefined && init.headers['RequestVerificationToken'] !== undefined) {
        delete init.headers['RequestVerificationToken'];
    }

    return originalFetch.apply(this, arguments);
};