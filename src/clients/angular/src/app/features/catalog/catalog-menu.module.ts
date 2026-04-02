import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';

import { CategoryMenuComponent } from './components/category-menu/category-menu.component';
import { CategoryNestedMenuComponent } from './components/category-nested-menu/category-nested-menu.component';

/** Eager slice so the navbar can show categories while `CatalogModule` stays lazy for routes. */
@NgModule({
  declarations: [CategoryMenuComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatMenuModule,
    CategoryNestedMenuComponent,
  ],
  exports: [CategoryMenuComponent],
})
export class CatalogMenuModule {}
