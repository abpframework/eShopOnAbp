export version="1.0.0"

az acr login --name volocr

docker tag eshoponabp/app-web:"${version}" ghcr.io/volosoft/eshoponabp/app-web:"${version}"
docker push ghcr.io/volosoft/eshoponabp/app-web:"${version}"

docker tag eshoponabp/app-authserver:"${version}" ghcr.io/volosoft/eshoponabp/app-authserver:"${version}"
docker push ghcr.io/volosoft/eshoponabp/app-authserver:"${version}"

docker tag eshoponabp/app-publicweb:"${version}" ghcr.io/volosoft/eshoponabp/app-publicweb:"${version}"
docker push ghcr.io/volosoft/eshoponabp/app-publicweb:"${version}"

docker tag eshoponabp/gateway-web:"${version}" ghcr.io/volosoft/eshoponabp/gateway-web:"${version}"
docker push ghcr.io/volosoft/eshoponabp/gateway-web:"${version}"

docker tag eshoponabp/gateway-web-public:"${version}" ghcr.io/volosoft/eshoponabp/gateway-web-public:"${version}"
docker push ghcr.io/volosoft/eshoponabp/gateway-web-public:"${version}"

docker tag eshoponabp/service-identity:"${version}" ghcr.io/volosoft/eshoponabp/service-identity:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-identity:"${version}"

docker tag eshoponabp/service-administration:"${version}" ghcr.io/volosoft/eshoponabp/service-administration:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-administration:"${version}"

docker tag eshoponabp/service-basket:"${version}" ghcr.io/volosoft/eshoponabp/service-basket:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-basket:"${version}"

docker tag eshoponabp/service-catalog:"${version}" ghcr.io/volosoft/eshoponabp/service-catalog:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-catalog:"${version}"

docker tag eshoponabp/service-ordering:"${version}" ghcr.io/volosoft/eshoponabp/service-ordering:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-ordering:"1.0.1"

docker tag eshoponabp/service-cmskit:"${version}" ghcr.io/volosoft/eshoponabp/service-cmskit:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-cmskit:"${version}"

docker tag eshoponabp/service-payment:"${version}" ghcr.io/volosoft/eshoponabp/service-payment:"${version}"
docker push ghcr.io/volosoft/eshoponabp/service-payment:"${version}"