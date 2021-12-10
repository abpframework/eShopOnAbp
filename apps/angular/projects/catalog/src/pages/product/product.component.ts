import { Component, OnInit } from '@angular/core';
import { ProductService } from '@catalog/proxy/products';
import { map } from 'rxjs/operators';
@Component({
  selector: 'lib-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  list$ = this.productService.getList().pipe(map(res => res.items));
  constructor(public readonly productService: ProductService) {}

  ngOnInit(): void {}
}
