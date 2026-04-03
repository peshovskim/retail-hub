import { createSelector } from '@ngrx/store';

import type { Product } from '../models/product.model';
import { catalogFeature } from './catalog.reducer';

/** Flat exports (existing consumers). */
export const selectCatalogCategories = catalogFeature.selectCategories;
export const selectCatalogLoading = catalogFeature.selectLoading;
export const selectCatalogError = catalogFeature.selectError;

export const selectCategoryMenu = catalogFeature.selectMenu;
export const selectCategoryMenuLoading = catalogFeature.selectMenuLoading;
export const selectCategoryMenuError = catalogFeature.selectMenuError;

export const selectCatalogProducts = catalogFeature.selectProducts;
export const selectCatalogProductsLoading = catalogFeature.selectProductsLoading;
export const selectCatalogProductsError = catalogFeature.selectProductsError;

/**
 * Zalary-style grouped selectors (`inboxEntriesQuery`) for one import surface.
 */
export const catalogQuery = {
  getCategories: catalogFeature.selectCategories,
  getLoading: catalogFeature.selectLoading,
  getError: catalogFeature.selectError,
  getMenu: catalogFeature.selectMenu,
  getMenuLoading: catalogFeature.selectMenuLoading,
  getMenuError: catalogFeature.selectMenuError,
  getProducts: catalogFeature.selectProducts,
  getProductsLoading: catalogFeature.selectProductsLoading,
  getProductsError: catalogFeature.selectProductsError,
};

export type CatalogProductsView =
  | { kind: 'loading' }
  | { kind: 'error'; message: string }
  | { kind: 'ok'; items: Product[] };

export const selectCatalogProductsView = createSelector(
  catalogFeature.selectProductsLoading,
  catalogFeature.selectProductsError,
  catalogFeature.selectProducts,
  (loading, error, items): CatalogProductsView => {
    if (loading) {
      return { kind: 'loading' };
    }
    if (error) {
      return { kind: 'error', message: error };
    }
    return { kind: 'ok', items };
  },
);
