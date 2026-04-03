import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRippleModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

import { CatalogRoutingModule } from './catalog-routing.module';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ProductDetailsPage } from './pages/product-details.page';
import { ProductListPage } from './pages/product-list.page';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [ProductListPage, ProductDetailsPage, ProductCardComponent],
  imports: [
    CommonModule,
    SharedModule,
    CatalogRoutingModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatSelectModule,
  ],
})
export class CatalogModule {}
