 # Pre-requirements

* Docker Desktop with Kubernetes enabled
* Install [NGINX ingress](https://kubernetes.github.io/ingress-nginx/deploy/) for k8s

    OR

    Install NGINX ingress using helm
```powershell
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm upgrade --install --version=4.0.19 ingress-nginx ingress-nginx/ingress-nginx
```
* Install [Helm](https://helm.sh/docs/intro/install/) for running helm charts


# How to run?

* Add entries to the hosts file (in Windows: `C:\Windows\System32\drivers\etc\hosts`, in linux and macos: `/etc/hosts` ):

````powershell
198.19.249.2 admin.eshoponabp.dev
198.19.249.2 eshoponabp.dev
198.19.249.2 account.eshoponabp.dev
198.19.249.2 identity.eshoponabp.dev
198.19.249.2 administration.eshoponabp.dev
198.19.249.2 basket.eshoponabp.dev
198.19.249.2 catalog.eshoponabp.dev
198.19.249.2 ordering.eshoponabp.dev
198.19.249.2 cmskit.eshoponabp.dev
198.19.249.2 payment.eshoponabp.dev
198.19.249.2 gateway-web.eshoponabp.dev
198.19.249.2 gateway-public.eshoponabp.dev
````
Once Helm is set up properly, add the repo as follows:

```console
helm repo add eshoponabp https://abpframework.github.io/abp-charts/
```
You can then run `helm search repo eshoponabp` to see the charts.

```console
 helm install eshop-st eshoponabp/eshoponabp
```

OR

* Run `build-images.ps1` or `build-images.sh` in the `build` directory.
* Run `deploy-staging.ps1` or `deploy-staging.sh` in the `helm-chart` directory. It is deployed with the `eshop` namespace.
* *You may wait ~30 seconds on first run for preparing the database*.
* Browse https://eshop-st-public-web for public and https://eshop-st-web for web application
* Username: `admin`, password: `1q2w3E*`.

# Running on HTTPS

You can also run the staging solution on your local kubernetes kluster with https. There are various ways to create self-signed certificate. 

## Installing mkcert
This guide will use mkcert to create self-signed certificates.

Follow the [installation guide](https://github.com/FiloSottile/mkcert#installation) to install mkcert.

## Creating mkcert Root CA
Use the command to create root (local) certificate authority for your certificates:
```powershell
mkcert -install
```

**Note:** all the certificates created by mkcert certificate creation will be trusted by local machine

## Run mkcert

Create certificate for the eshopOnAbp domains using the mkcert command below:
```powershell
mkcert "eshoponabp.dev" "*.eshoponabp.dev"
```

At the end of the output you will see something like

The certificate is at "./eshop-st-web+10.pem" and the key at "./eshop-st-web+10-key.pem"

Copy the cert name and key name below to create tls secret

```powershell
kubectl create namespace eshop
kubectl create secret tls -n eshop eshop-wildcard-tls --cert=./eshoponabp.dev+1.pem  --key=./eshoponabp.dev+1-key.pem
```
