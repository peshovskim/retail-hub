import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CategoryMenuComponent } from './components/category-menu/category-menu.component';

/** Eager slice so the navbar can show categories while `CatalogModule` stays lazy for routes. */
@NgModule({
  declarations: [CategoryMenuComponent],
  imports: [CommonModule, RouterModule],
  exports: [CategoryMenuComponent],
})
export class CatalogMenuModule {}
