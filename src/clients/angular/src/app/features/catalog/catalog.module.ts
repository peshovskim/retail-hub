import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { CatalogRoutingModule } from './catalog-routing.module';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ProductFilterComponent } from './components/product-filter/product-filter.component';
import { ProductDetailsPage } from './pages/product-details.page';
import { ProductListPage } from './pages/product-list.page';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    ProductListPage,
    ProductDetailsPage,
    ProductCardComponent,
    ProductFilterComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CatalogRoutingModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatRippleModule,
  ],
})
export class CatalogModule {}
