import { ModuleWithProviders, NgModule } from '@angular/core';
import { ORDERING_ROUTE_PROVIDERS } from './providers/route.provider';


@NgModule()
export class OrderConfigModule {
  static forRoot(): ModuleWithProviders<OrderConfigModule> {
    return {
      ngModule: OrderConfigModule,
      providers: [ORDERING_ROUTE_PROVIDERS],
    };
  }
}
