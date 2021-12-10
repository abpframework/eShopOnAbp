import { ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
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

  selected: ProductDto;

  isModalVisible: boolean;

  modalBusy = false;
  constructor(
    public readonly productService: ProductService,
    public readonly list: ListService,
    private confirmationService: ConfirmationService
  ) {
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

  onEdit(product: ProductDto) {
    this.selected = product;
    this.openModal();
  }

  onCreate() {
    this.selected = {} as ProductDto;
    this.openModal();
  }

  openModal() {
    this.isModalVisible = true;
  }

  onDelete(product: ProductDto) {
    this.confirmationService
      .warn('AbpCatalog::ProductDeletionConfirmationMessage', 'AbpCatalog::AreYouSure', {
        messageLocalizationParams: [product.name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.productService.delete(product.id).subscribe(() => this.list.get());
        }
      });
  }
}
