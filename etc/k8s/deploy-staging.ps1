helm upgrade --install eshop-st eshoponabp --namespace eshop --create-namespace

helm upgrade --install eshop-az eshoponabp -f ./azure/values.azure.yaml --namespace eshop  