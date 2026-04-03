import type { Product } from './product.model';

/** Matches `ProductListSort` on the API (query binding is case-insensitive). */
export type ProductListSort = 'nameAsc' | 'nameDesc' | 'priceAsc' | 'priceDesc';

/** Query parameters for `GET /api/catalog/products` (omit fields you do not need). */
export interface ProductListParams {
  search?: string;
  categoryIds?: string[];
  priceMin?: number;
  priceMax?: number;
  sort?: ProductListSort;
  page?: number;
  pageSize?: number;
}

/** Matches `ProductListResult` from the API (camelCase JSON). */
export interface ProductListResponse {
  items: Product[];
  totalCount: number;
}
