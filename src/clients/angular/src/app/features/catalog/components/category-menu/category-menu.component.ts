import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { catalogActions } from '../../store/catalog.actions';
import {
  selectCatalogCategories,
  selectCatalogError,
  selectCatalogLoading,
} from '../../store/catalog.selectors';

@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrl: './category-menu.component.scss',
  standalone: false,
})
export class CategoryMenuComponent implements OnInit {
  protected categories$;
  protected loading$;
  protected error$;

  constructor(private readonly store: Store) {
    this.categories$ = this.store.select(selectCatalogCategories);
    this.loading$ = this.store.select(selectCatalogLoading);
    this.error$ = this.store.select(selectCatalogError);
  }

  ngOnInit(): void {
    this.store.dispatch(catalogActions.loadCategories());
  }
}
