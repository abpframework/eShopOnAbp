import { ModuleWithProviders, NgModule } from '@angular/core';
import { SAAS_SERVİCE_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class SaasServiceConfigModule {
  static forRoot(): ModuleWithProviders<SaasServiceConfigModule> {
    return {
      ngModule: SaasServiceConfigModule,
      providers: [SAAS_SERVİCE_ROUTE_PROVIDERS],
    };
  }
}
