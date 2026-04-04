import { createFeature, createReducer, on } from '@ngrx/store';

import type { Cart } from '../models/cart.model';
import { cartActions } from './cart.actions';

export const CART_FEATURE_KEY = 'cart' as const;

export interface CartState {
  cart: Cart | null;
  initialized: boolean;
  loading: boolean;
  mutationBusy: boolean;
  error: string | null;
}

const initialState: CartState = {
  cart: null,
  initialized: false,
  loading: false,
  mutationBusy: false,
  error: null,
};

export const cartReducer = createReducer(
  initialState,
  on(cartActions.bootstrap, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(cartActions.bootstrapSuccess, (state, { cart }) => ({
    ...state,
    cart,
    initialized: true,
    loading: false,
    error: null,
  })),
  on(cartActions.bootstrapFailure, (state, { error }) => ({
    ...state,
    initialized: true,
    loading: false,
    error,
  })),
  on(cartActions.addToCart, (state) => ({
    ...state,
    mutationBusy: true,
    error: null,
  })),
  on(cartActions.applyCartFromServer, (state, { cart }) => ({
    ...state,
    cart,
    loading: false,
    mutationBusy: false,
    error: null,
  })),
  on(cartActions.addToCartFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
  on(cartActions.refreshCart, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(cartActions.refreshCartFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),
  on(cartActions.updateLineQuantityFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
  on(cartActions.removeLineFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
  on(cartActions.updateLineQuantity, (state) => ({
    ...state,
    mutationBusy: true,
    error: null,
  })),
  on(cartActions.updateLineQuantityFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
  on(cartActions.removeLine, (state) => ({
    ...state,
    mutationBusy: true,
    error: null,
  })),
  on(cartActions.removeLineFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
  on(cartActions.placeOrder, (state) => ({
    ...state,
    mutationBusy: true,
    error: null,
  })),
  on(cartActions.placeOrderCompleted, (state, { cart }) => ({
    ...state,
    cart,
    mutationBusy: false,
    error: null,
  })),
  on(cartActions.placeOrderFailure, (state, { error }) => ({
    ...state,
    mutationBusy: false,
    error,
  })),
);

export const cartFeature = createFeature({
  name: CART_FEATURE_KEY,
  reducer: cartReducer,
});
