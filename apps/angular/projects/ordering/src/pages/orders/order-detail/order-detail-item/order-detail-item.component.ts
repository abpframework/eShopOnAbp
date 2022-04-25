import { Component, Input } from '@angular/core';

@Component({
  selector: 'lib-order-detail-item',
  template: `
    <div class="row">
      <div class="col-3">
        {{ label | abpLocalization }}
      </div>
      <b class="col-9">
        <ng-content></ng-content>
      </b>
    </div>
  `,
  styles: [],
})
export class OrderDetailItemComponent {
  @Input()
  label = '';
}
