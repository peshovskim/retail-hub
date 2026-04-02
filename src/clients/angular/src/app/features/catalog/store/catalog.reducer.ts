import { createFeature, createReducer, on } from '@ngrx/store';

import type { Category, CategoryMenuNode } from '../models/category.model';
import { catalogActions } from './catalog.actions';

export const CATALOG_FEATURE_KEY = 'catalog' as const;

export interface CatalogState {
  categories: Category[];
  loading: boolean;
  error: string | null;
  menu: CategoryMenuNode[];
  menuLoading: boolean;
  menuError: string | null;
}

const initialState: CatalogState = {
  categories: [],
  loading: false,
  error: null,
  menu: [],
  menuLoading: false,
  menuError: null,
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
  on(catalogActions.loadCategoryMenu, (state) => ({
    ...state,
    menuLoading: true,
    menuError: null,
  })),
  on(catalogActions.loadCategoryMenuSuccess, (state, { menu }) => ({
    ...state,
    menu,
    menuLoading: false,
  })),
  on(catalogActions.loadCategoryMenuFailure, (state, { error }) => ({
    ...state,
    menuLoading: false,
    menuError: error,
  })),
);

export const catalogFeature = createFeature({
  name: CATALOG_FEATURE_KEY,
  reducer: catalogReducer,
});
