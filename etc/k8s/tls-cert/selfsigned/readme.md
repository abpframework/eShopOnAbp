`helm install cert-manager jetstack/cert-manager --namespace cert-manager --create-namespace --version v1.4.0 --set installCRDs=true`
`kubectl create ns eshop`
`kubectl apply -f .\selfsigned\issuer.yaml`


```powershell
kubectl create secret tls eshop-demo-tls --cert=eshop-st-cert.pem --key=eshop-st-cert.key -n ingress-nginx
```