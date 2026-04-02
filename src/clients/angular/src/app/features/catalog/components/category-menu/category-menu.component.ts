import { ChangeDetectorRef, Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Store } from '@ngrx/store';

import { catalogActions } from '../../store/catalog.actions';
import {
  selectCategoryMenu,
  selectCategoryMenuError,
  selectCategoryMenuLoading,
} from '../../store/catalog.selectors';

@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrl: './category-menu.component.scss',
  standalone: false,
})
export class CategoryMenuComponent implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);

  protected menu$;
  protected loading$;
  protected error$;

  constructor(private readonly store: Store) {
    this.menu$ = this.store.select(selectCategoryMenu);
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
