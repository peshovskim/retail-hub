import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap } from 'rxjs';

import { CatalogApiService } from '../services/catalog-api.service';
import { catalogActions } from './catalog.actions';

/**
 * `inject()` fields must run before `createEffect` fields: class field initializers run
 * top-to-bottom before the constructor, so `createEffect(() => this.actions$.pipe(...))`
 * would otherwise see `actions$` as undefined.
 */
@Injectable()
export class CatalogEffects {
  private readonly actions$ = inject(Actions);
  private readonly api = inject(CatalogApiService);

  loadCategories$ = createEffect(() =>
    this.actions$.pipe(
      ofType(catalogActions.loadCategories),
      switchMap(() =>
        this.api.getCategories().pipe(
          map((categories) => catalogActions.loadCategoriesSuccess({ categories })),
          catchError((err: unknown) =>
            of(
              catalogActions.loadCategoriesFailure({
                error: err instanceof Error ? err.message : 'Unknown error',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  loadCategoryMenu$ = createEffect(() =>
    this.actions$.pipe(
      ofType(catalogActions.loadCategoryMenu),
      switchMap(() =>
        this.api.getCategoryMenu().pipe(
          map((menu) => catalogActions.loadCategoryMenuSuccess({ menu })),
          catchError((err: unknown) =>
            of(
              catalogActions.loadCategoryMenuFailure({
                error: err instanceof Error ? err.message : 'Unknown error',
              }),
            ),
          ),
        ),
      ),
    ),
  );
}
