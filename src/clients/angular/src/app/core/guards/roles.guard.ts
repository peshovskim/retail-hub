import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';

import { AuthService } from '../services/auth.service';

/**
 * Requires route `data.roles` (e.g. `{ roles: ['Admin'] }`). User must have at least one listed role.
 */
export const rolesGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const required = (route.data['roles'] as string[] | undefined) ?? [];
  if (required.length === 0) {
    return true;
  }

  return auth.hydrateIfNeeded().pipe(
    map(() => {
      const ok = required.some((role) => auth.hasRole(role));
      return ok ? true : router.createUrlTree(['/catalog']);
    }),
  );
};
