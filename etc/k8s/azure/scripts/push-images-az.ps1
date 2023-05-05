param ($version='latest')

az acr login --name volosoft

docker tag eshoponabp/app-web:$version volosoft.azurecr.io/eshoponabp/app-web:$version
docker push volosoft.azurecr.io/eshoponabp/app-web:$version
docker tag volosoft.azurecr.io/eshoponabp/app-web:$version volosoft.azurecr.io/eshoponabp/dbmigrator:latest

docker tag eshoponabp/app-authserver:$version volosoft.azurecr.io/eshoponabp/app-authserver:$version
docker push volosoft.azurecr.io/eshoponabp/app-authserver:$version
docker tag volosoft.azurecr.io/eshoponabp/app-authserver:$version volosoft.azurecr.io/eshoponabp/app-authserver:latest

docker tag eshoponabp/app-publicweb:$version volosoft.azurecr.io/eshoponabp/app-publicweb:$version
docker push volosoft.azurecr.io/eshoponabp/app-publicweb:$version
docker tag volosoft.azurecr.io/eshoponabp/app-publicweb:$version volosoft.azurecr.io/eshoponabp/app-publicweb:latest

docker tag eshoponabp/gateway-web:$version volosoft.azurecr.io/eshoponabp/gateway-web:$version
docker push volosoft.azurecr.io/eshoponabp/gateway-web:$version
docker tag volosoft.azurecr.io/eshoponabp/gateway-web:$version volosoft.azurecr.io/eshoponabp/gateway-web:latest

docker tag eshoponabp/gateway-web-public:$version volosoft.azurecr.io/eshoponabp/gateway-web-public:$version
docker push volosoft.azurecr.io/eshoponabp/gateway-web-public:$version
docker tag volosoft.azurecr.io/eshoponabp/gateway-web-public:$version volosoft.azurecr.io/eshoponabp/gateway-web-public:latest

docker tag eshoponabp/service-identity:$version volosoft.azurecr.io/eshoponabp/service-identity:$version
docker push volosoft.azurecr.io/eshoponabp/service-identity:$version
docker tag volosoft.azurecr.io/eshoponabp/service-identity:$version volosoft.azurecr.io/eshoponabp/service-identity:latest

docker tag eshoponabp/service-administration:$version volosoft.azurecr.io/eshoponabp/service-administration:$version
docker push volosoft.azurecr.io/eshoponabp/service-administration:$version
docker tag volosoft.azurecr.io/eshoponabp/service-administration:$version volosoft.azurecr.io/eshoponabp/service-administration:latest

docker tag eshoponabp/service-catalog:$version volosoft.azurecr.io/eshoponabp/service-catalog:$version
docker push volosoft.azurecr.io/eshoponabp/service-catalog:$version
docker tag volosoft.azurecr.io/eshoponabp/service-catalog:$version volosoft.azurecr.io/eshoponabp/service-catalog:latest

docker tag eshoponabp/service-basket:$version volosoft.azurecr.io/eshoponabp/service-basket:$version
docker push volosoft.azurecr.io/eshoponabp/service-basket:$version
docker tag volosoft.azurecr.io/eshoponabp/service-basket:$version volosoft.azurecr.io/eshoponabp/service-basket:latest

docker tag eshoponabp/service-payment:$version volosoft.azurecr.io/eshoponabp/service-payment:$version
docker push volosoft.azurecr.io/eshoponabp/service-payment:$version
docker tag volosoft.azurecr.io/eshoponabp/service-payment:$version volosoft.azurecr.io/eshoponabp/service-payment:latest

docker tag eshoponabp/service-ordering:$version volosoft.azurecr.io/eshoponabp/service-ordering:$version
docker push volosoft.azurecr.io/eshoponabp/service-ordering:$version
docker tag volosoft.azurecr.io/eshoponabp/service-ordering:$version volosoft.azurecr.io/eshoponabp/service-ordering:latest

docker tag eshoponabp/service-cmskit:$version volosoft.azurecr.io/eshoponabp/service-cmskit:$version
docker push volosoft.azurecr.io/eshoponabp/service-cmskit:$version
docker tag volosoft.azurecr.io/eshoponabp/service-cmskit:$version volosoft.azurecr.io/eshoponabp/service-cmskit:latest



