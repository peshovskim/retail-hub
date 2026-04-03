import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed, toObservable, toSignal } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs';

import type { CategoryMenuNode } from '../models/category.model';
import type { ProductListParams, ProductListSort } from '../models/product-list.model';
import type { CatalogProductsView } from '../store/catalog.selectors';
import { CatalogFacade } from '../store/catalog.facade';

export type ShopSortOption = 'name-asc' | 'name-desc' | 'price-asc' | 'price-desc';

function flattenCategoryNodes(nodes: CategoryMenuNode[]): CategoryMenuNode[] {
  const out: CategoryMenuNode[] = [];
  const walk = (list: CategoryMenuNode[]) => {
    for (const n of list) {
      out.push(n);
      if (n.children?.length) {
        walk(n.children);
      }
    }
  };
  walk(nodes);
  return out;
}

function buildCategoryNameMap(nodes: CategoryMenuNode[]): Map<string, string> {
  const map = new Map<string, string>();
  const walk = (list: CategoryMenuNode[]) => {
    for (const n of list) {
      map.set(n.id, n.name);
      if (n.children?.length) {
        walk(n.children);
      }
    }
  };
  walk(nodes);
  return map;
}

function shopSortToApi(sort: ShopSortOption): ProductListSort {
  switch (sort) {
    case 'name-desc':
      return 'nameDesc';
    case 'price-asc':
      return 'priceAsc';
    case 'price-desc':
      return 'priceDesc';
    case 'name-asc':
    default:
      return 'nameAsc';
  }
}

@Component({
  selector: 'app-product-list-page',
  templateUrl: './product-list.page.html',
  styleUrl: './product-list.page.scss',
  standalone: false,
})
export class ProductListPage {
  private readonly catalog = inject(CatalogFacade);

  constructor() {
    this.catalog.loadCategoryMenu();
    this.reloadProducts();

    toObservable(this.searchText)
      .pipe(
        debounceTime(350),
        distinctUntilChanged(),
        skip(1),
        takeUntilDestroyed(),
      )
      .subscribe(() => this.reloadProducts());
  }

  protected readonly productsView = toSignal(this.catalog.productsView$, {
    initialValue: { kind: 'loading' } as CatalogProductsView,
  });

  protected readonly menu = toSignal(this.catalog.menu$, { initialValue: [] as CategoryMenuNode[] });

  protected readonly menuLoading = toSignal(this.catalog.menuLoading$, { initialValue: false });

  protected readonly sortOption = signal<ShopSortOption>('name-asc');
  protected readonly selectedCategoryIds = signal(new Set<string>());
  protected readonly priceMax = signal<number | null>(null);
  protected readonly searchText = signal('');

  protected readonly flatCategories = computed(() => flattenCategoryNodes(this.menu()));

  protected readonly categoryNameMap = computed(() => buildCategoryNameMap(this.menu()));

  protected categoryLabel(categoryId: string): string {
    return this.categoryNameMap().get(categoryId) ?? 'General';
  }

  protected isCategorySelected(id: string): boolean {
    return this.selectedCategoryIds().has(id);
  }

  protected toggleCategory(id: string, checked: boolean): void {
    const next = new Set(this.selectedCategoryIds());
    if (checked) {
      next.add(id);
    } else {
      next.delete(id);
    }
    this.selectedCategoryIds.set(next);
    this.reloadProducts();
  }

  protected clearFilters(): void {
    this.selectedCategoryIds.set(new Set());
    this.priceMax.set(null);
    this.searchText.set('');
    this.reloadProducts();
  }

  protected onPriceMaxChange(event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    if (raw === '') {
      this.priceMax.set(null);
    } else {
      const n = Number(raw);
      this.priceMax.set(Number.isFinite(n) ? n : null);
    }
    this.reloadProducts();
  }

  protected hasActiveFilters(): boolean {
    return (
      this.selectedCategoryIds().size > 0 ||
      this.priceMax() != null ||
      this.searchText().trim().length > 0
    );
  }

  protected setSort(value: unknown): void {
    const v = String(value) as ShopSortOption;
    if (v === 'name-asc' || v === 'name-desc' || v === 'price-asc' || v === 'price-desc') {
      this.sortOption.set(v);
      this.reloadProducts();
    }
  }

  protected onSearchInput(event: Event): void {
    this.searchText.set((event.target as HTMLInputElement).value);
  }

  private buildParams(): ProductListParams {
    const sort = shopSortToApi(this.sortOption());
    const categoryIds = [...this.selectedCategoryIds()];
    const priceMax = this.priceMax();
    const search = this.searchText().trim();
    const params: ProductListParams = { sort };
    if (categoryIds.length > 0) {
      params.categoryIds = categoryIds;
    }
    if (priceMax != null && !Number.isNaN(priceMax) && priceMax >= 0) {
      params.priceMax = priceMax;
    }
    if (search.length > 0) {
      params.search = search;
    }
    return params;
  }

  private reloadProducts(): void {
    this.catalog.loadProducts(this.buildParams());
  }
}
