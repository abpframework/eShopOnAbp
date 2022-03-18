import { NgModule } from '@angular/core';
import { OrderingRoutingModule } from './ordering-routing.module';
import { OrderStatusComponent } from './lib/components/order-status/order-status.component';


@NgModule({
  declarations: [
    OrderStatusComponent
  ],
  imports: [
    OrderingRoutingModule
  ],
    exports: [
        OrderStatusComponent
    ]
})
export class OrderingModule {
}
