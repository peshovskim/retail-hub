import { createActionGroup, emptyProps, props } from '@ngrx/store';

import type { Category, CategoryMenuNode } from '../models/category.model';
import type { Product } from '../models/product.model';

export const catalogActions = createActionGroup({
  source: 'Catalog',
  events: {
    loadCategories: emptyProps(),
    loadCategoriesSuccess: props<{ categories: Category[] }>(),
    loadCategoriesFailure: props<{ error: string }>(),

    loadCategoryMenu: emptyProps(),
    loadCategoryMenuSuccess: props<{ menu: CategoryMenuNode[] }>(),
    loadCategoryMenuFailure: props<{ error: string }>(),

    loadProducts: emptyProps(),
    loadProductsSuccess: props<{ products: Product[] }>(),
    loadProductsFailure: props<{ error: string }>(),
  },
});
