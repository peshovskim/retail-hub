import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from '../core/guards/auth.guard';
import { rolesGuard } from '../core/guards/roles.guard';
import { AccountPage } from '../features/user-area/pages/account.page';
import { AdminPage } from '../features/user-area/pages/admin.page';
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
      {
        path: 'auth',
        loadChildren: () => import('../features/auth/auth.module').then((m) => m.AuthModule),
      },
      {
        path: 'account',
        component: AccountPage,
        canActivate: [authGuard],
      },
      {
        path: 'admin',
        component: AdminPage,
        canActivate: [authGuard, rolesGuard],
        data: { roles: ['Admin'] },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule {}
