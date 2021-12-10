import { NgModule } from '@angular/core';
import { CatalogRoutingModule } from './catalog-routing.module';
import { CatalogComponent } from './catalog.component';

@NgModule({
  declarations: [CatalogComponent],
  imports: [CatalogRoutingModule],
  exports: [CatalogComponent],
})
export class CatalogModule {}
