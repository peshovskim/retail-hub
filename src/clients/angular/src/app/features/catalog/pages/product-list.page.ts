import { Component } from '@angular/core';
import { Store } from '@ngrx/store';

import {
  selectCatalogCategories,
  selectCatalogError,
  selectCatalogLoading,
} from '../store/catalog.selectors';

@Component({
  selector: 'app-product-list-page',
  templateUrl: './product-list.page.html',
  styleUrl: './product-list.page.scss',
  standalone: false,
})
export class ProductListPage {
  protected categories$;
  protected loading$;
  protected error$;

  constructor(private readonly store: Store) {
    this.categories$ = this.store.select(selectCatalogCategories);
    this.loading$ = this.store.select(selectCatalogLoading);
    this.error$ = this.store.select(selectCatalogError);
  }
}
