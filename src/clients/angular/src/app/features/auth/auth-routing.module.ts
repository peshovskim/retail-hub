import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { guestGuard } from '../../core/guards/guest.guard';
import { LoginPage } from './pages/login.page';
import { RegisterPage } from './pages/register.page';

const routes: Routes = [
  { path: 'login', component: LoginPage, canActivate: [guestGuard] },
  { path: 'register', component: RegisterPage, canActivate: [guestGuard] },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
