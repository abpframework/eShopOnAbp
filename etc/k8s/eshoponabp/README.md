# abp-charts

# eShopOnAbp
This project is a reference for who want to build microservice solutions with the ABP Framework.

## Pre-requirement

* [Helm](https://helm.sh) must be installed to use the charts.
Please refer to Helm's [documentation](https://helm.sh/docs/) to get started.
* Install [NGINX ingress](https://kubernetes.github.io/ingress-nginx/deploy/) for k8s or Install NGINX ingress using helm
  ```powershell
  helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
  helm repo update

  helm upgrade --install --version=4.0.19 ingress-nginx ingress-nginx/ingress-nginx
  ```

Once Helm is set up properly, add the repo as follows:

```console
helm repo add abp-charts https://abpframework.github.io/abp-charts/eshoponabp
```

Initial authentication data (redirectURIs etc) are seeded based on **eshop-st** name and **eshop** namespace for the deployment.

## Configuring HTTPS

You can also run the staging solution on your local Kubernetes Cluster with HTTPS. There are various ways to create self-signed certificate. 

### Installing mkcert
This guide will use mkcert to create self-signed certificates.

Follow the [installation guide](https://github.com/FiloSottile/mkcert#installation) to install mkcert.

### Creating mkcert Root CA
Use the command to create root (local) certificate authority for your certificates:
```powershell
mkcert -install
```

**Note:** all the certificates created by mkcert certificate creation will be trusted by local machine

### Run mkcert

Create certificate for the eshopOnAbp domains using the mkcert command below:
```powershell
mkcert "eshop-st-web" "eshop-st-public-web" "eshop-st-authserver" "eshop-st-identity" "eshop-st-administration" "eshop-st-basket" "eshop-st-catalog" "eshop-st-ordering" "eshop-st-cmskit" "eshop-st-payment" "eshop-st-gateway-web" "eshop-st-gateway-web-public"
```

At the end of the output you will see something like

The certificate is at "./eshop-st-web+10.pem" and the key at "./eshop-st-web+10-key.pem"

Copy the cert name and key name below to create tls secret

```powershell
kubectl create namespace eshop
kubectl create secret tls -n eshop eshop-wildcard-tls --cert=./eshop-st-web+10.pem --key=./eshop-st-web+10-key.pem
```

## How to run?

* Add entries to the hosts file (in Windows: `C:\Windows\System32\drivers\etc\hosts`, in linux and macos: `/etc/hosts` ):

  ````powershell
  127.0.0.1 eshop-st-web
  127.0.0.1 eshop-st-public-web
  127.0.0.1 eshop-st-authserver
  127.0.0.1 eshop-st-identity
  127.0.0.1 eshop-st-administration
  127.0.0.1 eshop-st-basket
  127.0.0.1 eshop-st-catalog
  127.0.0.1 eshop-st-ordering
  127.0.0.1 eshop-st-cmskit
  127.0.0.1 eshop-st-payment
  127.0.0.1 eshop-st-gateway-web
  127.0.0.1 eshop-st-gateway-web-public
  ````

* Run `helm upgrade --install eshop-st abp-charts/eshoponabp --namespace eshop --create-namespace`
* *You may wait ~30 seconds on first run for preparing the database*.
* Browse https://eshop-st-public-web for public and https://eshop-st-web for web application
* Username: `admin`, password: `1q2w3E*`.
