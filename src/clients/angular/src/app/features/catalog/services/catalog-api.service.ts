import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../../../core/tokens';
import type { Category, CategoryMenuNode } from '../models/category.model';
import type { ProductListParams, ProductListResponse } from '../models/product-list.model';
import type { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class CatalogApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiBaseUrl}/api/catalog/categories`);
  }

  getCategoryMenu(): Observable<CategoryMenuNode[]> {
    return this.http.get<CategoryMenuNode[]>(`${this.apiBaseUrl}/api/catalog/categories/menu`);
  }

  getProducts(params?: ProductListParams): Observable<ProductListResponse>;
  getProducts(params: HttpParams): Observable<ProductListResponse>;
  getProducts(params?: ProductListParams | HttpParams): Observable<ProductListResponse> {
    const httpParams =
      params === undefined
        ? new HttpParams()
        : params instanceof HttpParams
          ? params
          : buildProductListHttpParams(params);
    return this.http.get<ProductListResponse>(`${this.apiBaseUrl}/api/catalog/products`, {
      params: httpParams,
    });
  }

  getProduct(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiBaseUrl}/api/catalog/products/${id}`);
  }
}

/** Builds query string parameters for the catalog products list; omits empty / undefined values. */
export function buildProductListHttpParams(criteria: ProductListParams): HttpParams {
  let params = new HttpParams();
  const search = criteria.search?.trim();
  if (search) {
    params = params.set('search', search);
  }
  if (criteria.categoryIds?.length) {
    for (const id of criteria.categoryIds) {
      if (id != null && id > 0) {
        params = params.append('categoryIds', String(id));
      }
    }
  }
  if (criteria.priceMin != null && !Number.isNaN(criteria.priceMin)) {
    params = params.set('priceMin', String(criteria.priceMin));
  }
  if (criteria.priceMax != null && !Number.isNaN(criteria.priceMax)) {
    params = params.set('priceMax', String(criteria.priceMax));
  }
  if (criteria.sort != null) {
    params = params.set('sort', criteria.sort);
  }
  if (criteria.page != null && !Number.isNaN(criteria.page)) {
    params = params.set('page', String(criteria.page));
  }
  if (criteria.pageSize != null && !Number.isNaN(criteria.pageSize)) {
    params = params.set('pageSize', String(criteria.pageSize));
  }
  return params;
}
