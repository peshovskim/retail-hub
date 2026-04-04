import { Injectable } from '@angular/core';

const CART_ID_KEY = 'retailHub.cartId';
const ANON_KEY = 'retailHub.cartSession';

@Injectable({ providedIn: 'root' })
export class CartSessionStorage {
  read(): { cartId: string | null; anonymousKey: string | null } {
    return {
      cartId: localStorage.getItem(CART_ID_KEY),
      anonymousKey: localStorage.getItem(ANON_KEY),
    };
  }

  save(cartId: string, anonymousKey: string): void {
    localStorage.setItem(CART_ID_KEY, cartId);
    localStorage.setItem(ANON_KEY, anonymousKey);
  }

  clear(): void {
    localStorage.removeItem(CART_ID_KEY);
    localStorage.removeItem(ANON_KEY);
  }
}
