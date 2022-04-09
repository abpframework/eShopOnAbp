# Notes

## Creating demo tls cert

```
kubectl create secret tls eshop-demo-tls \
  --cert=eshop-st-cert.pem \
  --key=eshop-st-key.pem
```

# Install cert-manager

`kubectl create namespace eshop`

Next, use the **`kubectl apply`** command and the **`yaml`** file available online to install the add-on:

```powershell
kubectl apply --validate=false -f https://github.com/jetstack/cert-manager/releases/download/v1.7.0/cert-manager.yaml
```

Now deploy the issuer:

```powershell
kubectl apply -f .\selfsigned\issuer.yaml
```

Now deploy the certificate:

```powershell
kubectl apply -f .\selfsigned\certificate.yaml
```

Check the certificate:

```powershell
kubectl describe certificate -n=eshop
```

Check the secrets:

```powershell
kubectl get secrets -n=eshop
```

You should be seeing `eshop-staging-tls`

To view information about the Secret, use the **`get secret`** command:

`kubectl get secret eshop-staging-tls -n cert-manager`
