import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import type { Category } from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CatalogApiService {
  constructor(private readonly http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    const base = environment.apiBaseUrl.replace(/\/$/, '');
    return this.http.get<Category[]>(`${base}/api/catalog/categories`);
  }
}
