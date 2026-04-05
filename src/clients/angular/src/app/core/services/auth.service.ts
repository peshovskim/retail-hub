import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, catchError, map, of, tap, throwError } from 'rxjs';

import type { AuthResponse, CurrentUser } from '../models/auth.model';
import { AuthApiService } from './auth-api.service';
import { AuthTokenStorage } from './auth-token.storage';
import { AuthUserCache } from './auth-user.cache';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(AuthApiService);
  private readonly tokenStorage = inject(AuthTokenStorage);
  private readonly userCache = inject(AuthUserCache);

  private readonly _user = signal<CurrentUser | null>(null);
  private readonly sessionRevision = signal(0);

  readonly user = this._user.asReadonly();
  readonly isAuthenticated = computed(() => {
    this.sessionRevision();
    return !!this.tokenStorage.getAccessToken();
  });

  constructor() {
    if (this.tokenStorage.getAccessToken()) {
      const cached = this.userCache.read();
      if (cached) {
        this._user.set(cached);
        this.sessionRevision.update((n) => n + 1);
      }
      this.refreshUser().subscribe({
        error: (err) => {
          if (AuthService.isUnauthorized(err)) {
            this.clearSession();
          }
        },
      });
    }
  }

  hasRole(role: string): boolean {
    return this._user()?.roles?.includes(role) ?? false;
  }

  hydrateIfNeeded(): Observable<void> {
    if (this._user() !== null) {
      return of(void 0);
    }
    if (!this.tokenStorage.getAccessToken()) {
      return of(void 0);
    }
    return this.refreshUser().pipe(
      map(() => void 0),
      catchError((err) => {
        if (AuthService.isUnauthorized(err)) {
          this.clearSession();
        }
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
    if (!this.tokenStorage.getAccessToken()) {
      return throwError(
        () => new HttpErrorResponse({ status: 401, statusText: 'Not authenticated' }),
      );
    }

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
    this.userCache.clear();
    this.setUser(null);
  }

  private setUser(user: CurrentUser | null): void {
    this._user.set(user);
    this.sessionRevision.update((n) => n + 1);
    if (user !== null) {
      this.userCache.save(user);
    }
  }

  /** Only clear stored credentials when the API rejects the token (not on network/CORS/config errors). */
  private static isUnauthorized(err: unknown): boolean {
    return err instanceof HttpErrorResponse && err.status === 401;
  }
}
