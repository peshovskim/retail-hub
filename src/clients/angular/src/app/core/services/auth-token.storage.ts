import { Injectable } from '@angular/core';

const ACCESS_TOKEN_KEY = 'retailHub.auth.accessToken';

@Injectable({ providedIn: 'root' })
export class AuthTokenStorage {
  getAccessToken(): string | null {
    try {
      return globalThis.localStorage?.getItem(ACCESS_TOKEN_KEY) ?? null;
    } catch {
      return null;
    }
  }

  setAccessToken(token: string): void {
    try {
      globalThis.localStorage?.setItem(ACCESS_TOKEN_KEY, token);
    } catch {
      /* storage unavailable */
    }
  }

  clearAccessToken(): void {
    try {
      globalThis.localStorage?.removeItem(ACCESS_TOKEN_KEY);
    } catch {
      /* storage unavailable */
    }
  }
}
