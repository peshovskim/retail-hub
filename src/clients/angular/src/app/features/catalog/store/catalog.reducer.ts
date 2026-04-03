import { createFeature, createReducer, on } from '@ngrx/store';

import type { Category, CategoryMenuNode } from '../models/category.model';
import type { Product } from '../models/product.model';
import { catalogActions } from './catalog.actions';

export const CATALOG_FEATURE_KEY = 'catalog' as const;

export interface CatalogState {
  categories: Category[];
  loading: boolean;
  error: string | null;
  menu: CategoryMenuNode[];
  menuLoading: boolean;
  menuError: string | null;
  products: Product[];
  productsLoading: boolean;
  productsError: string | null;
}

const initialState: CatalogState = {
  categories: [],
  loading: false,
  error: null,
  menu: [],
  menuLoading: false,
  menuError: null,
  products: [],
  productsLoading: false,
  productsError: null,
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
  on(catalogActions.loadProducts, (state) => ({
    ...state,
    productsLoading: true,
    productsError: null,
  })),
  on(catalogActions.loadProductsSuccess, (state, { products }) => ({
    ...state,
    products,
    productsLoading: false,
  })),
  on(catalogActions.loadProductsFailure, (state, { error }) => ({
    ...state,
    productsLoading: false,
    productsError: error,
  })),
);

export const catalogFeature = createFeature({
  name: CATALOG_FEATURE_KEY,
  reducer: catalogReducer,
});
