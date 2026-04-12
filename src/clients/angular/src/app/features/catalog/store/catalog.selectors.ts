import { createSelector } from '@ngrx/store';

import type { Product } from '../models/product.model';
import { catalogFeature } from './catalog.reducer';

/** Flat exports (existing consumers). */
export const selectCatalogCategories = catalogFeature.selectCategories;
export const selectCatalogLoading = catalogFeature.selectLoading;
export const selectCatalogError = catalogFeature.selectError;

export const selectCategoryMenu = catalogFeature.selectMenu;
export const selectCategoryMenuLoaded = catalogFeature.selectMenuLoaded;
export const selectCategoryMenuLoading = catalogFeature.selectMenuLoading;
export const selectCategoryMenuError = catalogFeature.selectMenuError;

export const selectCatalogProducts = catalogFeature.selectProducts;
export const selectCatalogProductsLoading = catalogFeature.selectProductsLoading;
export const selectCatalogProductsError = catalogFeature.selectProductsError;
export const selectProductListParams = catalogFeature.selectProductListParams;
export const selectProductListTotalCount = catalogFeature.selectProductListTotalCount;

/**
 * Zalary-style grouped selectors (`inboxEntriesQuery`) for one import surface.
 */
export const catalogQuery = {
  getCategories: catalogFeature.selectCategories,
  getLoading: catalogFeature.selectLoading,
  getError: catalogFeature.selectError,
  getMenu: catalogFeature.selectMenu,
  getMenuLoaded: catalogFeature.selectMenuLoaded,
  getMenuLoading: catalogFeature.selectMenuLoading,
  getMenuError: catalogFeature.selectMenuError,
  getProducts: catalogFeature.selectProducts,
  getProductsLoading: catalogFeature.selectProductsLoading,
  getProductsError: catalogFeature.selectProductsError,
  getProductListParams: catalogFeature.selectProductListParams,
  getProductListTotalCount: catalogFeature.selectProductListTotalCount,
};

export type CatalogProductsView =
  | { kind: 'loading' }
  | { kind: 'error'; message: string }
  | { kind: 'ok'; items: Product[]; totalCount: number };

export const selectCatalogProductsView = createSelector(
  catalogFeature.selectProductsLoading,
  catalogFeature.selectProductsError,
  catalogFeature.selectProducts,
  catalogFeature.selectProductListTotalCount,
  (loading, error, items, totalCount): CatalogProductsView => {
    // Avoid full-page spinner on every refetch: keep showing previous results while loading.
    if (loading && items.length === 0) {
      return { kind: 'loading' };
    }
    if (error) {
      return { kind: 'error', message: error };
    }
    return { kind: 'ok', items, totalCount };
  },
);
