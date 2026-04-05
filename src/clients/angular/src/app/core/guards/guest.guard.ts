import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthTokenStorage } from '../services/auth-token.storage';

/** Redirect signed-in users away from login/register. */
export const guestGuard: CanActivateFn = () => {
  const tokens = inject(AuthTokenStorage);
  const router = inject(Router);
  if (!tokens.getAccessToken()) {
    return true;
  }
  return router.createUrlTree(['/catalog']);
};
