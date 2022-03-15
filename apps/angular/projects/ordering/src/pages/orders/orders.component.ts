import { Component, OnInit } from '@angular/core';
import {  OrderService } from '../../lib/proxy/orders';
import { OrderViewModel, toOrderViewModel } from '../../lib';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { ListService } from '@abp/ng.core';

@Component({
  selector: 'lib-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  providers: [ListService],
})
export class OrdersComponent implements OnInit {
  constructor(private service: OrderService,
              public list: ListService,
              private confirmationService: ConfirmationService) { }
  selectedOrder: OrderViewModel | undefined;
  isModalVisible = false;
  items: OrderViewModel[];
  count = 0;

  ngOnInit(): void {

    const ordersStreamCreator = query => this.service.getListPaged(query);

    this.list.hookToQuery(ordersStreamCreator).subscribe(response => {
      this.items = toOrderViewModel(response.items);
      this.count = response.totalCount;
    });
  }

  openModal(order: OrderViewModel) {
    if (!order){
      return;
    }
    this.selectedOrder = order;
    this.isModalVisible = true;
  }

  closeModal(isVisible: boolean){
    if (isVisible){
      return;
    }
    this.selectedOrder = null;
    this.isModalVisible = false;
  }

  setAsShipped(row: OrderViewModel) {
    this.confirmationService
      .warn('AbpOrdering::WillSetAsShipped', { key: '::AreYouSure', defaultValue: 'Are you sure?' })
      .subscribe((status) => {
        if (status !== Confirmation.Status.confirm) {
          return;
        }
        this.service.setAsShipped(row.id).subscribe(() => {
          this.list.get();
        });
      });
  }
  setAsCancelled(row: OrderViewModel){
    this.confirmationService
      .warn('AbpOrdering::WillSetAsCancelled', { key: '::AreYouSure', defaultValue: 'Are you sure?' })
      .subscribe((status``) => {
        if (status !== Confirmation.Status.confirm) {
          return;
        }
        this.service.setAsCancelled(row.id, { paymentRequestId: undefined, paymentRequestStatus: undefined}).subscribe(() => {
          this.list.get();
        });
      })
    ;
  }
}
