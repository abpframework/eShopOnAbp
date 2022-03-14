import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { OrderDto } from '../../../lib/proxy/orders';

@Component({
  selector: 'lib-order-detail',
  templateUrl: './order-detail.component.html'
})
export class OrderDetailComponent implements OnInit {

  @Input()
  visible: boolean;
  @Input()
  order: OrderDto | undefined;

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

}
