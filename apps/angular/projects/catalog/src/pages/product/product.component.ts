import { ListService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { ProductDto, ProductService } from '@catalog/proxy/products';

@Component({
  selector: 'lib-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
  providers: [ListService],
})
export class ProductComponent implements OnInit {
  items: ProductDto[] = [];
  count = 0;
  constructor(public readonly productService: ProductService, public readonly list: ListService) {
    // TODO: this is an example of paging
    this.list.maxResultCount = 2;
  }

  ngOnInit(): void {
    const productStreamCreator = query => this.productService.getListPaged(query);

    this.list.hookToQuery(productStreamCreator).subscribe(response => {
      this.items = response.items;
      this.count = response.totalCount;
    });
  }

  onEdit(row: ProductDto) {
    // this.productService.edit(row);
  }

  onDelete(row: ProductDto) {}
}
