import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap } from 'rxjs';

import { CatalogApiService } from '../services/catalog-api.service';
import { catalogActions } from './catalog.actions';

@Injectable()
export class CatalogEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly api: CatalogApiService,
  ) {}

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
}
