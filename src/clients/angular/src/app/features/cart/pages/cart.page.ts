import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { CartFacade } from '../store/cart.facade';

@Component({
  selector: 'app-cart-page',
  templateUrl: './cart.page.html',
  styleUrl: './cart.page.scss',
  standalone: false,
})
export class CartPage {
  protected readonly facade = inject(CartFacade);
  private readonly router = inject(Router);

  protected readonly cart = toSignal(this.facade.cart$, { initialValue: null });
  protected readonly loading = toSignal(this.facade.loading$, { initialValue: false });
  protected readonly mutationBusy = toSignal(this.facade.mutationBusy$, { initialValue: false });
  protected readonly error = toSignal(this.facade.error$, { initialValue: null });

  protected continueShopping(): void {
    void this.router.navigate(['/catalog']);
  }

  protected changeQty(productId: string, event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    const n = parseInt(raw, 10);
    if (!Number.isFinite(n) || n < 0 || n > 99) {
      return;
    }
    this.facade.updateLineQuantity(productId, n);
  }

  protected remove(productId: string): void {
    this.facade.removeLine(productId);
  }
}
