import { ChangeDetectorRef, Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Store } from '@ngrx/store';
import { map } from 'rxjs';

import type { CategoryMenuNode } from '../../models/category.model';
import { catalogActions } from '../../store/catalog.actions';
import {
  selectCategoryMenu,
  selectCategoryMenuError,
  selectCategoryMenuLoading,
} from '../../store/catalog.selectors';

const TOOLBAR_EXCLUDED_CATEGORY_SLUGS = new Set(['clothing', 'luggage-and-barrows']);

function filterCategoryMenuForToolbar(nodes: CategoryMenuNode[]): CategoryMenuNode[] {
  return nodes
    .filter((n) => !TOOLBAR_EXCLUDED_CATEGORY_SLUGS.has(n.slug))
    .map((n) => ({
      ...n,
      children: filterCategoryMenuForToolbar(n.children),
    }));
}

@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrl: './category-menu.component.scss',
  standalone: false,
})
export class CategoryMenuComponent implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);

  protected menuForToolbar$;
  protected loading$;
  protected error$;

  constructor(private readonly store: Store) {
    this.menuForToolbar$ = this.store
      .select(selectCategoryMenu)
      .pipe(map((nodes) => filterCategoryMenuForToolbar(nodes)));
    this.loading$ = this.store.select(selectCategoryMenuLoading);
    this.error$ = this.store.select(selectCategoryMenuError);
  }

  ngOnInit(): void {
    this.store.dispatch(catalogActions.loadCategoryMenu());
    this.loading$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((loading) => {
        if (!loading) {
          queueMicrotask(() => this.cdr.detectChanges());
        }
      });
  }
}
