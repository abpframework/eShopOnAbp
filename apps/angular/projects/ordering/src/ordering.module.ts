import { NgModule } from '@angular/core';
import { OrderingRoutingModule } from './ordering-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { TopSellingComponent } from './pages/dashboard/components/top-selling/top-selling.component';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [DashboardComponent, TopSellingComponent],
  imports: [OrderingRoutingModule, CommonModule],
  exports: [],
})
export class OrderingModule {}
