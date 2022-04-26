import { Component, Input } from '@angular/core';
import { OrderStatusDto } from '../../../../lib/proxy/orders';

@Component({
  selector: 'app-order-status-chart',
  templateUrl: './order-status-chart.component.html',
})
export class OrderStatusChartComponent {
  @Input()
  set data(value: OrderStatusDto[]) {
    this.chartData.labels = [...value.map(x => x.orderStatus)];
    this.chartData.datasets = [
      {
        label: 'Order Statuses',
        data: [...value.map(x => x.countOfStatusOrder)],
        backgroundColor: ['#fdcb6e', '#0984e3', '#ff7675'],
      },
    ];
  }

  chartData = {
    labels: [],
    datasets: [],
  };
}
