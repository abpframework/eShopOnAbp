import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { SaasServiceComponent } from './components/saas-service.component';
import { SaasServiceRoutingModule } from './saas-service-routing.module';

@NgModule({
  declarations: [SaasServiceComponent],
  imports: [CoreModule, ThemeSharedModule, SaasServiceRoutingModule],
  exports: [SaasServiceComponent],
})
export class SaasServiceModule {
  static forChild(): ModuleWithProviders<SaasServiceModule> {
    return {
      ngModule: SaasServiceModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<SaasServiceModule> {
    return new LazyModuleFactory(SaasServiceModule.forChild());
  }
}
