import { AccountConfigModule } from '@abp/ng.account/config';
import { CoreModule, EnvironmentService, NAVIGATE_TO_MANAGE_PROFILE } from '@abp/ng.core';
import { registerLocale } from '@abp/ng.core/locale';
import { IdentityConfigModule } from '@abp/ng.identity/config';
import { SettingManagementConfigModule } from '@abp/ng.setting-management/config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management/config';
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { AccountLayoutModule } from '@abp/ng.theme.lepton-x/account';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule, inject } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CatalogConfigModule } from '@eshoponabp/catalog/config';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { OrderingConfigModule } from '@eshoponabp/ordering/config';
import { AbpOAuthModule } from '@abp/ng.oauth';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule.forRoot({
      environment,
      registerLocaleFn: registerLocale(),
    }),
    AbpOAuthModule.forRoot(),
    ThemeSharedModule.forRoot(),
    AccountConfigModule.forRoot(),
    IdentityConfigModule.forRoot(),
    TenantManagementConfigModule.forRoot(),
    SettingManagementConfigModule.forRoot(),
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
    AccountLayoutModule.forRoot(),
    CatalogConfigModule.forRoot(),
    OrderingConfigModule.forRoot(),
  ],
  declarations: [AppComponent],
  providers: [
    APP_ROUTE_PROVIDER,
    {
      provide: NAVIGATE_TO_MANAGE_PROFILE,
      useFactory: () => {
        const environment = inject(EnvironmentService);
        return () => {
          location.href = `${environment.getIssuer()}account`;
        };
      },
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
