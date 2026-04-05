import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, catchError, map, of, tap } from 'rxjs';

import type { AuthResponse, CurrentUser } from '../models/auth.model';
import { AuthApiService } from './auth-api.service';
import { AuthTokenStorage } from './auth-token.storage';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(AuthApiService);
  private readonly tokenStorage = inject(AuthTokenStorage);

  private readonly _user = signal<CurrentUser | null>(null);
  private readonly sessionRevision = signal(0);

  readonly user = this._user.asReadonly();
  readonly isAuthenticated = computed(() => {
    this.sessionRevision();
    return !!this.tokenStorage.getAccessToken();
  });

  constructor() {
    if (this.tokenStorage.getAccessToken()) {
      this.refreshUser().subscribe({
        error: () => this.clearSession(),
      });
    }
  }

  hasRole(role: string): boolean {
    return this._user()?.roles?.includes(role) ?? false;
  }

  /**
   * Ensures `user` is populated when a token exists (e.g. for role checks after hard refresh).
   */
  hydrateIfNeeded(): Observable<void> {
    if (this._user() !== null) {
      return of(void 0);
    }
    if (!this.tokenStorage.getAccessToken()) {
      return of(void 0);
    }
    return this.refreshUser().pipe(
      map(() => void 0),
      catchError(() => {
        this.clearSession();
        return of(void 0);
      }),
    );
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.api.login({ email, password }).pipe(tap((r) => this.applyAuthResponse(r)));
  }

  register(email: string, password: string): Observable<AuthResponse> {
    return this.api.register({ email, password }).pipe(tap((r) => this.applyAuthResponse(r)));
  }

  refreshUser(): Observable<CurrentUser> {
    return this.api.getCurrentUser().pipe(tap((u) => this.setUser(u)));
  }

  logout(): void {
    this.clearSession();
  }

  private applyAuthResponse(response: AuthResponse): void {
    this.tokenStorage.setAccessToken(response.accessToken);
    this.setUser({
      uid: response.userId,
      email: response.email,
      roles: [...response.roles],
    });
  }

  private clearSession(): void {
    this.tokenStorage.clearAccessToken();
    this.setUser(null);
  }

  private setUser(user: CurrentUser | null): void {
    this._user.set(user);
    this.sessionRevision.update((n) => n + 1);
  }
}
