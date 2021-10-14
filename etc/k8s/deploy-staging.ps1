param ($version='latest')

helm upgrade --install eshop eshoponabp --namespace eshop --create-namespace --set global.eventHubImageVersion=$version