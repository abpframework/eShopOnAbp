 ### Pre-requirements

* Docker Desktop with Kubernetes enabled
* Install [NGINX ingress](https://kubernetes.github.io/ingress-nginx/deploy/) for k8s
OR
* Install NGINX ingress using helm
```powershell
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm install ingress-nginx ingress-nginx/ingress-nginx
```
* Install [Helm](https://helm.sh/docs/intro/install/) for running helm charts


### How to run?

* Add entries to the hosts file (in Windows: `C:\Windows\System32\drivers\etc\hosts`):

````powershell
127.0.0.1 eshop-st-web
127.0.0.1 eshop-st-public-web
127.0.0.1 eshop-st-authserver
127.0.0.1 eshop-st-identity
127.0.0.1 eshop-st-administration
127.0.0.1 eshop-st-saas
127.0.0.1 eshop-st-gateway-web
127.0.0.1 eshop-st-gateway-public-web
127.0.0.1 eshop-st-gateway-internal
````

* Run `build-images.ps1` in the `scripts` directory.
* Run `deploy-staging.ps1` in the `helm-chart` directory. It is deployed with the `eventhub` namespace.
* *You may wait ~30 seconds on first run for preparing the database*.
* Browse https://eshoponabp-public-web and https://eshoponabp-authserver
* Username: `admin`, password: `1q2w3E*`.
