import { inject, Injectable } from '@angular/core';
import { Store } from '@ngrx/store';

import { cartActions } from './cart.actions';
import { cartQuery } from './cart.selectors';

@Injectable({ providedIn: 'root' })
export class CartFacade {
  private readonly store = inject(Store);

  readonly cart$ = this.store.select(cartQuery.getCart);
  readonly initialized$ = this.store.select(cartQuery.getInitialized);
  readonly loading$ = this.store.select(cartQuery.getLoading);
  readonly mutationBusy$ = this.store.select(cartQuery.getMutationBusy);
  readonly error$ = this.store.select(cartQuery.getError);
  readonly itemCount$ = this.store.select(cartQuery.getItemCount);

  bootstrap(): void {
    this.store.dispatch(cartActions.bootstrap());
  }

  addToCart(productId: string, quantity: number): void {
    this.store.dispatch(cartActions.addToCart({ productId, quantity }));
  }

  refreshCart(): void {
    this.store.dispatch(cartActions.refreshCart());
  }

  updateLineQuantity(productId: string, quantity: number): void {
    this.store.dispatch(cartActions.updateLineQuantity({ productId, quantity }));
  }

  removeLine(productId: string): void {
    this.store.dispatch(cartActions.removeLine({ productId }));
  }
}
