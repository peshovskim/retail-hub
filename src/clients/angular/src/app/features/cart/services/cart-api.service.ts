import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL } from '../../../core/tokens';
import type { Cart, CartSession } from '../models/cart.model';

/** Must stay aligned with `RetailHub.Api.Controllers.CartController.CartSessionHeaderName`. */
export const CART_SESSION_HEADER = 'X-Cart-Session';

@Injectable({ providedIn: 'root' })
export class CartApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  createSession(anonymousKey?: string | null): Observable<CartSession> {
    let headers = new HttpHeaders();
    if (anonymousKey) {
      headers = headers.set(CART_SESSION_HEADER, anonymousKey);
    }
    return this.http.post<CartSession>(
      `${this.apiBaseUrl}/api/cart/session`,
      {},
      { headers },
    );
  }

  getCart(cartId: string): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiBaseUrl}/api/cart/${cartId}`);
  }

  addItem(cartId: string, productId: string, quantity: number): Observable<Cart> {
    return this.http.post<Cart>(`${this.apiBaseUrl}/api/cart/items`, {
      cartId,
      productId,
      quantity,
    });
  }

  updateItemQuantity(cartId: string, productId: string, quantity: number): Observable<Cart> {
    return this.http.patch<Cart>(`${this.apiBaseUrl}/api/cart/items/${productId}`, {
      cartId,
      quantity,
    });
  }

  removeItem(cartId: string, productId: string): Observable<Cart> {
    return this.http.delete<Cart>(`${this.apiBaseUrl}/api/cart/items/${productId}`, {
      params: { cartId },
    });
  }
}
