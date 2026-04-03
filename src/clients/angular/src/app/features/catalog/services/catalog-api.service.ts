import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../../../core/tokens';
import type { Category, CategoryMenuNode } from '../models/category.model';
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

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiBaseUrl}/api/catalog/products`);
  }
}
