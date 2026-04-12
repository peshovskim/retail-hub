import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { catchError, map, mergeMap, of, switchMap, take, tap, withLatestFrom } from 'rxjs';

import type { Cart } from '../models/cart.model';
import { CartApiService } from '../services/cart-api.service';
import { OrdersApiService } from '../services/orders-api.service';
import { CartSessionStorage } from '../services/cart-session.storage';
import { cartActions } from './cart.actions';
import { selectCart } from './cart.selectors';

function clearedCartSnapshot(cart: Cart): Cart {
  return {
    ...cart,
    lines: [],
    subtotal: 0,
    itemCount: 0,
  };
}

function extractErrorMessage(err: unknown, fallback: string): string {
  if (err instanceof HttpErrorResponse && err.error && typeof err.error === 'object' && 'message' in err.error) {
    return String((err.error as { message?: string }).message);
  }
  if (err instanceof Error) {
    return err.message;
  }
  return fallback;
}

@Injectable()
export class CartEffects {
  private readonly actions$ = inject(Actions);
  private readonly api = inject(CartApiService);
  private readonly ordersApi = inject(OrdersApiService);
  private readonly router = inject(Router);
  private readonly storage = inject(CartSessionStorage);
  private readonly store = inject(Store);

  /** Initial load: create/restore session from storage, then fetch the cart. */
  bootstrap$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.bootstrap),
      switchMap(() => {
        const { anonymousKey } = this.storage.read();
        return this.api.createSession(anonymousKey ?? undefined).pipe(
          tap((s) => this.storage.save(s.anonymousKey)),
          switchMap((s) => this.api.getCart(s.cartId)),
          map((cart) => cartActions.bootstrapSuccess({ cart })),
          catchError((err: unknown) =>
            of(
              cartActions.bootstrapFailure({
                error: extractErrorMessage(err, 'Unable to start cart session.'),
              }),
            ),
          ),
        );
      }),
    ),
  );

  addToCart$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.addToCart),
      withLatestFrom(this.store.select(selectCart)),
      mergeMap(([{ productId, quantity }, cart]) => {
        const ensureCartId$ =
          cart?.id != null
            ? of(cart.id)
            : this.api.createSession(this.storage.read().anonymousKey ?? undefined).pipe(
                tap((s) => this.storage.save(s.anonymousKey)),
                map((s) => s.cartId),
              );

        return ensureCartId$.pipe(
          switchMap((cartId) => this.api.addItem(cartId, productId, quantity)),
          map((c) => cartActions.applyCartFromServer({ cart: c })),
          catchError((err: unknown) =>
            of(cartActions.addToCartFailure({ error: extractErrorMessage(err, 'Could not add to cart.') })),
          ),
        );
      }),
    ),
  );

  refreshCart$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.refreshCart),
      switchMap(() =>
        this.store.select(selectCart).pipe(
          take(1),
          switchMap((cart) => {
            if (!cart?.id) {
              return of(cartActions.refreshCartFailure({ error: 'No cart loaded.' }));
            }
            return this.api.getCart(cart.id).pipe(
              map((c) => cartActions.applyCartFromServer({ cart: c })),
              catchError((err: unknown) =>
                of(
                  cartActions.refreshCartFailure({
                    error: extractErrorMessage(err, 'Unable to refresh cart.'),
                  }),
                ),
              ),
            );
          }),
        ),
      ),
    ),
  );

  updateLineQuantity$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.updateLineQuantity),
      withLatestFrom(this.store.select(selectCart)),
      switchMap(([{ productId, quantity }, cart]) => {
        if (!cart?.id) {
          return of(cartActions.updateLineQuantityFailure({ error: 'No cart loaded.' }));
        }
        return this.api.updateItemQuantity(cart.id, productId, quantity).pipe(
          map((c) => cartActions.applyCartFromServer({ cart: c })),
          catchError((err: unknown) =>
            of(
              cartActions.updateLineQuantityFailure({
                error: extractErrorMessage(err, 'Could not update quantity.'),
              }),
            ),
          ),
        );
      }),
    ),
  );

  removeLine$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.removeLine),
      withLatestFrom(this.store.select(selectCart)),
      switchMap(([{ productId }, cart]) => {
        if (!cart?.id) {
          return of(cartActions.removeLineFailure({ error: 'No cart loaded.' }));
        }
        return this.api.removeItem(cart.id, productId).pipe(
          map((c) => cartActions.applyCartFromServer({ cart: c })),
          catchError((err: unknown) =>
            of(cartActions.removeLineFailure({ error: extractErrorMessage(err, 'Could not remove item.') })),
          ),
        );
      }),
    ),
  );

  placeOrder$ = createEffect(() =>
    this.actions$.pipe(
      ofType(cartActions.placeOrder),
      withLatestFrom(this.store.select(selectCart)),
      switchMap(([, cart]) => {
        if (!cart?.id) {
          return of(cartActions.placeOrderFailure({ error: 'No cart loaded.' }));
        }
        return this.ordersApi.placeOrder({ cartId: cart.id }).pipe(
          switchMap((order) =>
            this.api.getCart(cart.id).pipe(
              map((c) => cartActions.placeOrderCompleted({ order, cart: c })),
              catchError(() =>
                of(
                  cartActions.placeOrderCompleted({
                    order,
                    cart: clearedCartSnapshot(cart),
                  }),
                ),
              ),
            ),
          ),
          catchError((err: unknown) =>
            of(cartActions.placeOrderFailure({ error: extractErrorMessage(err, 'Could not place order.') })),
          ),
        );
      }),
    ),
  );

  placeOrderCompletedNavigate$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(cartActions.placeOrderCompleted),
        tap(({ order }) => void this.router.navigate(['/cart', 'confirmation', order.id])),
      ),
    { dispatch: false },
  );
}
