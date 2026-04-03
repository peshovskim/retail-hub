import { Injectable, signal } from '@angular/core';

/**
 * Catalog product search text shown in the main navbar; the product list debounces
 * and passes it to the API.
 */
@Injectable({ providedIn: 'root' })
export class CatalogToolbarSearchService {
  readonly text = signal('');

  clear(): void {
    this.text.set('');
  }
}
