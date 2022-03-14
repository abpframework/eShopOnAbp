import { Component, OnInit } from '@angular/core';
import { OrderDto, OrderService } from '../../lib/proxy/orders';

@Component({
  selector: 'lib-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  constructor(private orderService: OrderService) { }
  list$ = this.orderService.getOrders({});
  selectedOrder: OrderDto | undefined;
  isModalVisible = false;
  ngOnInit(): void {}


  openModal(id: string) {
    this.isModalVisible = true;
  }
}
