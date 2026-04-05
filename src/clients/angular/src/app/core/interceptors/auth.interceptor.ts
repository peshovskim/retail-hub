import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthTokenStorage } from '../services/auth-token.storage';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private readonly tokens = inject(AuthTokenStorage);

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.tokens.getAccessToken();
    if (!token || req.headers.has('Authorization')) {
      return next.handle(req);
    }
    return next.handle(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
  }
}
