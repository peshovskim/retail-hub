import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProductDetailsPage } from './pages/product-details.page';
import { ProductListPage } from './pages/product-list.page';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: ProductListPage },
  { path: 'category/:slug', component: ProductListPage },
  { path: 'product/:id', component: ProductDetailsPage },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
