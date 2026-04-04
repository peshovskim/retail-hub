import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainLayoutPage } from './pages/main-layout.page';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutPage,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'catalog' },
      {
        path: 'catalog',
        loadChildren: () =>
          import('../features/catalog/catalog.module').then((m) => m.CatalogModule),
      },
      {
        path: 'cart',
        loadChildren: () => import('../features/cart/cart.module').then((m) => m.CartModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule {}
