import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { CartFacade } from '../store/cart.facade';

@Component({
  selector: 'app-checkout-page',
  templateUrl: './checkout.page.html',
  styleUrl: './checkout.page.scss',
  standalone: false,
})
export class CheckoutPage {
  protected readonly facade = inject(CartFacade);
  private readonly router = inject(Router);

  protected readonly cart = toSignal(this.facade.cart$, { initialValue: null });
  protected readonly loading = toSignal(this.facade.loading$, { initialValue: false });
  protected readonly initialized = toSignal(this.facade.initialized$, { initialValue: false });
  protected readonly mutationBusy = toSignal(this.facade.mutationBusy$, { initialValue: false });
  protected readonly error = toSignal(this.facade.error$, { initialValue: null });

  protected backToCart(): void {
    void this.router.navigate(['/cart']);
  }

  protected submitOrder(): void {
    this.facade.placeOrder();
  }
}
