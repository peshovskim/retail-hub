import { Injectable, signal } from '@angular/core';

/**
 * Toolbar search: `draft` is what the user types; `committed` is sent to the API
 * after Search is clicked or Enter is pressed (`applySearch`).
 */
@Injectable({ providedIn: 'root' })
export class CatalogToolbarSearchService {
  readonly draft = signal('');
  readonly committed = signal('');

  /** Copies draft into committed (call from Search button / Enter). */
  applySearch(): void {
    this.committed.set(this.draft());
  }

  clear(): void {
    this.draft.set('');
    this.committed.set('');
  }
}
