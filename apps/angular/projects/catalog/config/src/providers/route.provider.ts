import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eCatalogRouteNames } from '../enums/route-names';

export const CATALOG_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/catalog',
        name: eCatalogRouteNames.Catalog,
        layout: eLayoutType.application,
        // TODO: find icon
        iconClass: 'fa fa-users',
        // TODO: add this policy
        // requiredPolicy: eCatalogPolicyNames.Catalog,
      },
      {
        path: '/catalog/products',
        name: eCatalogRouteNames.ProductManagement,
        parentName: eCatalogRouteNames.Catalog,
        order: 1,
        // TODO: add this policy
        //  requiredPolicy: eCatalogPolicyNames.ProductManagement,
      },
    ]);
  };
}
