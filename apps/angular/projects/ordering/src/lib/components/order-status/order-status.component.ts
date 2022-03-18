import {ChangeDetectionStrategy, Component, Input} from '@angular/core';
import {orderStatusOptions} from '../../proxy/orders';

@Component({
  selector: 'lib-order-status',
  template: `
    {{statusText}}
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderStatusComponent {

  @Input()
  set id(v: number) {
    this.statusText = this.idToText(v);
  }

  options = orderStatusOptions;

  statusText: string;

  private idToText(id: number): string | null {
    console.log('hello', new Date().toLocalISOString())
    const item = this.options.find(x => x.value === id);

    if (!item) {
      return null;
    }
    return item.key;
  }


}
