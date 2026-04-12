import { Injectable } from '@angular/core';

const ANON_KEY = 'retailHub.cartSession';

@Injectable({ providedIn: 'root' })
export class CartSessionStorage {
  read(): { anonymousKey: string | null } {
    return {
      anonymousKey: localStorage.getItem(ANON_KEY),
    };
  }

  save(anonymousKey: string): void {
    localStorage.setItem(ANON_KEY, anonymousKey);
  }

  clear(): void {
    localStorage.removeItem(ANON_KEY);
  }
}
