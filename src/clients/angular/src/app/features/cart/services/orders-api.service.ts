import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../../../core/tokens';
import type { Order, PlaceOrderRequest } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrdersApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  placeOrder(request: PlaceOrderRequest): Observable<Order> {
    return this.http.post<Order>(`${this.apiBaseUrl}/api/orders`, request);
  }

  getOrder(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.apiBaseUrl}/api/orders/${orderId}`);
  }
}
