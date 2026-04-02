import { createFeature, createReducer, on } from '@ngrx/store';

import type { Category } from '../models/category.model';
import { catalogActions } from './catalog.actions';

export const CATALOG_FEATURE_KEY = 'catalog' as const;

export interface CatalogState {
  categories: Category[];
  loading: boolean;
  error: string | null;
}

const initialState: CatalogState = {
  categories: [],
  loading: false,
  error: null,
};

export const catalogReducer = createReducer(
  initialState,
  on(catalogActions.loadCategories, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(catalogActions.loadCategoriesSuccess, (state, { categories }) => ({
    ...state,
    categories,
    loading: false,
  })),
  on(catalogActions.loadCategoriesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),
);

export const catalogFeature = createFeature({
  name: CATALOG_FEATURE_KEY,
  reducer: catalogReducer,
});
