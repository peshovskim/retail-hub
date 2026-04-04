import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CartPage } from './pages/cart.page';
import { CheckoutPage } from './pages/checkout.page';
import { OrderConfirmationPage } from './pages/order-confirmation.page';

const routes: Routes = [
  { path: '', component: CartPage },
  { path: 'checkout', component: CheckoutPage },
  { path: 'confirmation/:orderId', component: OrderConfirmationPage },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CartRoutingModule {}
