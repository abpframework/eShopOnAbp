param ($version='latest')

helm upgrade --install eshoponabp eshoponabp --namespace eshop --create-namespace --set global.eventHubImageVersion=$version