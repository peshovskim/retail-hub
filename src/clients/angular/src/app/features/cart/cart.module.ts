import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { SharedModule } from '../../shared/shared.module';
import { CartRoutingModule } from './cart-routing.module';
import { CartPage } from './pages/cart.page';
import { CheckoutPage } from './pages/checkout.page';
import { OrderConfirmationPage } from './pages/order-confirmation.page';

@NgModule({
  declarations: [CartPage, CheckoutPage, OrderConfirmationPage],
  imports: [
    CommonModule,
    SharedModule,
    CartRoutingModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
})
export class CartModule {}
