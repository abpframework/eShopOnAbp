import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product.component';

@NgModule({
  declarations: [ProductComponent],
  imports: [CommonModule, ProductRoutingModule, ThemeSharedModule, CoreModule],
})
export class ProductModule {}
