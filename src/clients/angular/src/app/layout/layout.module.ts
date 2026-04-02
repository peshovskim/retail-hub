import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

import { CatalogMenuModule } from '../features/catalog/catalog-menu.module';
import { SharedModule } from '../shared/shared.module';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { LayoutRoutingModule } from './layout-routing.module';
import { MainLayoutPage } from './pages/main-layout.page';

@NgModule({
  declarations: [MainLayoutPage, NavbarComponent, SidebarComponent, FooterComponent],
  imports: [
    CommonModule,
    SharedModule,
    LayoutRoutingModule,
    CatalogMenuModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule,
  ],
})
export class LayoutModule {}
