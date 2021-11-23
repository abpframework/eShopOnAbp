# Notes

## Creating demo tls cert

```
kubectl create secret tls eshop-demo-tls \
  --cert=eshop-st-cert.pem \
  --key=eshop-st-key.pem
```
