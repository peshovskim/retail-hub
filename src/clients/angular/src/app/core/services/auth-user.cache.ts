import { Injectable } from '@angular/core';

import type { CurrentUser } from '../models/auth.model';

const KEY = 'retailHub.auth.userProfile';

@Injectable({ providedIn: 'root' })
export class AuthUserCache {
  read(): CurrentUser | null {
    try {
      const raw = globalThis.localStorage?.getItem(KEY);
      if (!raw) {
        return null;
      }
      const parsed = JSON.parse(raw) as Partial<CurrentUser>;
      if (typeof parsed?.uid !== 'string' || typeof parsed?.email !== 'string' || !Array.isArray(parsed?.roles)) {
        return null;
      }
      return { uid: parsed.uid, email: parsed.email, roles: [...parsed.roles] };
    } catch {
      return null;
    }
  }

  save(user: CurrentUser): void {
    try {
      globalThis.localStorage?.setItem(KEY, JSON.stringify(user));
    } catch {
      /* ignore */
    }
  }

  clear(): void {
    try {
      globalThis.localStorage?.removeItem(KEY);
    } catch {
      /* ignore */
    }
  }
}
