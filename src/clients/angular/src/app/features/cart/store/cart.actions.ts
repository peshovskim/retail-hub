import { createActionGroup, emptyProps, props } from '@ngrx/store';

import type { Cart } from '../models/cart.model';
import type { Order } from '../models/order.model';

export const cartActions = createActionGroup({
  source: 'Cart',
  events: {
    bootstrap: emptyProps(),
    bootstrapSuccess: props<{ cart: Cart }>(),
    bootstrapFailure: props<{ error: string }>(),

    addToCart: props<{ productId: string; quantity: number }>(),
    addToCartFailure: props<{ error: string }>(),

    refreshCart: emptyProps(),
    refreshCartFailure: props<{ error: string }>(),

    updateLineQuantity: props<{ productId: string; quantity: number }>(),
    updateLineQuantityFailure: props<{ error: string }>(),

    removeLine: props<{ productId: string }>(),
    removeLineFailure: props<{ error: string }>(),

    /** Merges server cart into state (used after mutations). */
    applyCartFromServer: props<{ cart: Cart }>(),

    placeOrder: emptyProps(),
    placeOrderCompleted: props<{ order: Order; cart: Cart }>(),
    placeOrderFailure: props<{ error: string }>(),
  },
});
