import { createActionGroup, emptyProps, props } from '@ngrx/store';

import type { Category } from '../models/category.model';

export const catalogActions = createActionGroup({
  source: 'Catalog',
  events: {
    loadCategories: emptyProps(),
    loadCategoriesSuccess: props<{ categories: Category[] }>(),
    loadCategoriesFailure: props<{ error: string }>(),
  },
});
