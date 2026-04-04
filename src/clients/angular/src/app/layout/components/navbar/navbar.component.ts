import { Component, inject } from '@angular/core';

import { CatalogToolbarSearchService } from '../../../features/catalog/services/catalog-toolbar-search.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
  standalone: false,
})
export class NavbarComponent {
  protected readonly catalogSearch = inject(CatalogToolbarSearchService);

  protected onSearchSubmit(event?: Event): void {
    event?.preventDefault();
    this.catalogSearch.applySearch();
  }
}
