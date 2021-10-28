param ($version='latest')

helm upgrade --install es-st eshoponabp --namespace eshop --create-namespace --set global.eshoponabpImageVersion=$version