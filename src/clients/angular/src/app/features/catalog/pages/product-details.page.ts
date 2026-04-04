import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';

import { CartFacade } from '../../cart/store/cart.facade';
import type { Product } from '../models/product.model';
import { CatalogApiService } from '../services/catalog-api.service';

export type ProductDetailVm =
  | { kind: 'loading' }
  | { kind: 'bad-route' }
  | { kind: 'not-found' }
  | { kind: 'error'; message: string }
  | { kind: 'ready'; product: Product };

@Component({
  selector: 'app-product-details-page',
  templateUrl: './product-details.page.html',
  styleUrl: './product-details.page.scss',
  standalone: false,
})
export class ProductDetailsPage {
  private readonly route = inject(ActivatedRoute);
  private readonly catalogApi = inject(CatalogApiService);
  private readonly cart = inject(CartFacade);

  protected readonly quantity = signal(1);

  protected readonly vm = toSignal(
    this.route.paramMap.pipe(
      map((params) => params.get('id')),
      switchMap((id) => {
        if (!id) {
          return of<ProductDetailVm>({ kind: 'bad-route' });
        }
        return this.catalogApi.getProduct(id).pipe(
          map((product): ProductDetailVm => ({ kind: 'ready', product })),
          catchError((err: unknown): Observable<ProductDetailVm> => {
            if (err instanceof HttpErrorResponse && err.status === 404) {
              return of({ kind: 'not-found' });
            }
            const message =
              err instanceof HttpErrorResponse && err.error && typeof err.error === 'object' && 'message' in err.error
                ? String((err.error as { message?: string }).message)
                : err instanceof Error
                  ? err.message
                  : 'Unable to load product.';
            return of({ kind: 'error', message });
          }),
          startWith<ProductDetailVm>({ kind: 'loading' }),
        );
      }),
    ),
    { initialValue: { kind: 'loading' } as ProductDetailVm },
  );

  protected decrementQty(): void {
    const q = this.quantity();
    if (q > 1) {
      this.quantity.set(q - 1);
    }
  }

  protected incrementQty(): void {
    const q = this.quantity();
    if (q < 99) {
      this.quantity.set(q + 1);
    }
  }

  protected onQtyInput(event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    const n = parseInt(raw, 10);
    if (Number.isFinite(n) && n >= 1 && n <= 99) {
      this.quantity.set(n);
    }
  }

  protected addToCart(product: Product): void {
    this.cart.addToCart(product.id, this.quantity());
  }

  protected descriptionText(product: Product): string {
    const d = product.description?.trim();
    if (d) {
      return d;
    }
    const s = product.shortDescription?.trim();
    if (s) {
      return s;
    }
    return 'No description is available for this product yet.';
  }
}
