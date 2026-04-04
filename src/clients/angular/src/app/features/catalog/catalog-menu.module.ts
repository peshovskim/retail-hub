import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';

import { CategoryMegaMenuComponent } from './components/category-mega-menu/category-mega-menu.component';
import { CategoryMenuComponent } from './components/category-menu/category-menu.component';

/** Eager slice so the navbar can show categories while `CatalogModule` stays lazy for routes. */
@NgModule({
  declarations: [CategoryMenuComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    CategoryMegaMenuComponent,
  ],
  exports: [CategoryMenuComponent],
})
export class CatalogMenuModule {}
