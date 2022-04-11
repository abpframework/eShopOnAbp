# Notes

## TODO: - section on mkcert

### Install mkcert root ca

```powershell
mkcert -install
```

### Run mkcert

```powershell
mkcert "eshop-st-web" "eshop-st-public-web" "eshop-st-authserver" "eshop-st-identity" "eshop-st-administration" "eshop-st-basket" "eshop-st-catalog" "eshop-st-ordering" "eshop-st-payment" "eshop-st-gateway-web" "eshop-st-gateway-web-public"
```

At the end of the output you will see something like

The certificate is at "./eshop-st-web+10.pem" and the key at "./eshop-st-web+10-key.pem"

Copy the cert name and key name below to create tls secret

```powershell
kubectl create namespace eshop
kubectl create secret tls -n eshop eshop-wildcard-tls --cert=./eshop-st-web+10.pem  --key=./eshop-st-web+10-key.pem
```
