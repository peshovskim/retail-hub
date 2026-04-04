import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';

import type { Order } from '../models/order.model';
import { OrdersApiService } from '../services/orders-api.service';

type LoadState =
  | { status: 'loading' }
  | { status: 'error'; message: string }
  | { status: 'ready'; order: Order };

@Component({
  selector: 'app-order-confirmation-page',
  templateUrl: './order-confirmation.page.html',
  styleUrl: './order-confirmation.page.scss',
  standalone: false,
})
export class OrderConfirmationPage {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly ordersApi = inject(OrdersApiService);

  private readonly load$ = this.route.paramMap.pipe(
    map((params) => params.get('orderId')),
    switchMap((orderId) => {
      if (!orderId) {
        return of<LoadState>({ status: 'error', message: 'Missing order reference.' });
      }
      return this.ordersApi.getOrder(orderId).pipe(
        map((order): LoadState => ({ status: 'ready', order })),
        catchError((err: unknown) => {
          const message =
            err && typeof err === 'object' && 'error' in err && err.error && typeof err.error === 'object'
              ? String((err.error as { message?: string }).message ?? 'Unable to load order.')
              : 'Unable to load order.';
          return of<LoadState>({ status: 'error', message });
        }),
        startWith<LoadState>({ status: 'loading' }),
      );
    }),
  );

  protected readonly state = toSignal(this.load$, { initialValue: { status: 'loading' } as LoadState });

  protected continueShopping(): void {
    void this.router.navigate(['/catalog']);
  }

  protected viewCart(): void {
    void this.router.navigate(['/cart']);
  }
}
