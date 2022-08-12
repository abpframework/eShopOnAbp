# abp-charts

# eShopOnAbp
This project is a reference for who want to build microservice solutions with the ABP Framework.

## Usage

[Helm](https://helm.sh) must be installed to use the charts.
Please refer to Helm's [documentation](https://helm.sh/docs/) to get started.

Once Helm is set up properly, add the repo as follows:

```console
helm repo add eshoponabp https://abpframework.github.io/abp-charts/eshoponabp
```

You can then run `helm search repo eshoponabp` to see the charts.


```console
 helm install eshop-st eshoponabp/eshoponabp
```
