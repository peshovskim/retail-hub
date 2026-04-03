import { Component, inject } from '@angular/core';
import { Observable } from 'rxjs';

import type { CatalogProductsView } from '../store/catalog.selectors';
import { CatalogFacade } from '../store/catalog.facade';

@Component({
  selector: 'app-product-list-page',
  templateUrl: './product-list.page.html',
  styleUrl: './product-list.page.scss',
  standalone: false,
})
export class ProductListPage {
  private readonly catalog = inject(CatalogFacade);

  constructor() {
    this.catalog.loadProducts();
  }

  /** All active products from the store (loaded via `CatalogEffects`). */
  protected readonly productsView$: Observable<CatalogProductsView> = this.catalog.productsView$;
}
