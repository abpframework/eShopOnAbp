import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { TopSellingDto } from '../../../../lib/proxy/order-items';

@Component({
  selector: 'app-top-selling',
  templateUrl: './top-selling.component.html',
  styleUrls: ['./top-selling.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopSellingComponent {
  @Input()
  topSellingProducts: Array<TopSellingDto> | undefined;
}
