import { catalogFeature } from './catalog.reducer';

export const selectCatalogCategories = catalogFeature.selectCategories;
export const selectCatalogLoading = catalogFeature.selectLoading;
export const selectCatalogError = catalogFeature.selectError;

export const selectCategoryMenu = catalogFeature.selectMenu;
export const selectCategoryMenuLoading = catalogFeature.selectMenuLoading;
export const selectCategoryMenuError = catalogFeature.selectMenuError;
