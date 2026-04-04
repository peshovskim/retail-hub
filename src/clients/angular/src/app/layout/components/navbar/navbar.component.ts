import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

import { CatalogToolbarSearchService } from '../../../features/catalog/services/catalog-toolbar-search.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
  standalone: false,
})
export class NavbarComponent {
  protected readonly catalogSearch = inject(CatalogToolbarSearchService);
  private readonly router = inject(Router);

  protected onSearchSubmit(event?: Event): void {
    event?.preventDefault();
    this.catalogSearch.applySearch();
    const search = this.catalogSearch.committed().trim();

    const path = this.router.url.split('?')[0];

    if (path.includes('/catalog/product/')) {
      void this.router.navigate(['/catalog'], {
        queryParams: search.length > 0 ? { search } : {},
        queryParamsHandling: '',
      });
      return;
    }

    const categoryMatch = /^\/catalog\/category\/([^/]+)$/.exec(path);
    if (categoryMatch) {
      void this.router.navigate(['/catalog/category', categoryMatch[1]], {
        queryParams: { search: search.length > 0 ? search : null },
        queryParamsHandling: 'merge',
        replaceUrl: true,
      });
      return;
    }

    if (path === '/catalog' || path === '/catalog/') {
      void this.router.navigate(['/catalog'], {
        queryParams: { search: search.length > 0 ? search : null },
        queryParamsHandling: 'merge',
        replaceUrl: true,
      });
      return;
    }

    void this.router.navigate(['/catalog'], {
      queryParams: search.length > 0 ? { search } : {},
      queryParamsHandling: '',
    });
  }
}
