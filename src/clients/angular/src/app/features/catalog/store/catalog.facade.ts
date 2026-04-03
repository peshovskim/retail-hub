import { inject, Injectable } from '@angular/core';
import { Store } from '@ngrx/store';

import { catalogActions } from './catalog.actions';
import { catalogQuery, selectCatalogProductsView } from './catalog.selectors';

/**
 * Thin API over catalog NgRx state (same role as Zalary's `InboxEntriesFacade`, without multi-tenant tokens).
 */
@Injectable({ providedIn: 'root' })
export class CatalogFacade {
  private readonly store = inject(Store);

  readonly categories$ = this.store.select(catalogQuery.getCategories);
  readonly loading$ = this.store.select(catalogQuery.getLoading);
  readonly error$ = this.store.select(catalogQuery.getError);

  readonly menu$ = this.store.select(catalogQuery.getMenu);
  readonly menuLoading$ = this.store.select(catalogQuery.getMenuLoading);
  readonly menuError$ = this.store.select(catalogQuery.getMenuError);

  readonly products$ = this.store.select(catalogQuery.getProducts);
  readonly productsLoading$ = this.store.select(catalogQuery.getProductsLoading);
  readonly productsError$ = this.store.select(catalogQuery.getProductsError);
  readonly productsView$ = this.store.select(selectCatalogProductsView);

  loadCategories(): void {
    this.store.dispatch(catalogActions.loadCategories());
  }

  loadCategoryMenu(): void {
    this.store.dispatch(catalogActions.loadCategoryMenu());
  }

  loadProducts(): void {
    this.store.dispatch(catalogActions.loadProducts());
  }
}
