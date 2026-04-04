import { createSelector } from '@ngrx/store';

import { cartFeature } from './cart.reducer';

export const selectCart = cartFeature.selectCart;
export const selectCartInitialized = cartFeature.selectInitialized;
export const selectCartLoading = cartFeature.selectLoading;
export const selectCartMutationBusy = cartFeature.selectMutationBusy;
export const selectCartError = cartFeature.selectError;

export const selectCartItemCount = createSelector(selectCart, (cart) => cart?.itemCount ?? 0);

export const cartQuery = {
  getCart: selectCart,
  getInitialized: selectCartInitialized,
  getLoading: selectCartLoading,
  getMutationBusy: selectCartMutationBusy,
  getError: selectCartError,
  getItemCount: selectCartItemCount,
};
