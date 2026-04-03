import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';

import type { CategoryMenuNode } from '../models/category.model';
import type { Product } from '../models/product.model';
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

function applyFiltersAndSort(
  items: Product[],
  opts: {
    selectedCategoryIds: Set<string>;
    priceMax: number | null;
    sort: ShopSortOption;
  },
): Product[] {
  let list = [...items];
  if (opts.selectedCategoryIds.size > 0) {
    list = list.filter((p) => opts.selectedCategoryIds.has(p.categoryId));
  }
  if (opts.priceMax != null && !Number.isNaN(opts.priceMax) && opts.priceMax > 0) {
    list = list.filter((p) => p.price <= opts.priceMax!);
  }
  list.sort((a, b) => {
    switch (opts.sort) {
      case 'name-desc':
        return b.name.localeCompare(a.name, undefined, { sensitivity: 'base' });
      case 'price-asc':
        return a.price - b.price;
      case 'price-desc':
        return b.price - a.price;
      case 'name-asc':
      default:
        return a.name.localeCompare(b.name, undefined, { sensitivity: 'base' });
    }
  });
  return list;
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
    this.catalog.loadProducts();
    this.catalog.loadCategoryMenu();
  }

  protected readonly productsView = toSignal(this.catalog.productsView$, {
    initialValue: { kind: 'loading' } as CatalogProductsView,
  });

  protected readonly menu = toSignal(this.catalog.menu$, { initialValue: [] as CategoryMenuNode[] });

  protected readonly menuLoading = toSignal(this.catalog.menuLoading$, { initialValue: false });

  protected readonly sortOption = signal<ShopSortOption>('name-asc');
  protected readonly selectedCategoryIds = signal(new Set<string>());
  protected readonly priceMax = signal<number | null>(null);

  protected readonly flatCategories = computed(() => flattenCategoryNodes(this.menu()));

  protected readonly categoryNameMap = computed(() => buildCategoryNameMap(this.menu()));

  protected readonly displayedProducts = computed(() => {
    const pv = this.productsView();
    if (pv.kind !== 'ok') {
      return [];
    }
    return applyFiltersAndSort(pv.items, {
      selectedCategoryIds: this.selectedCategoryIds(),
      priceMax: this.priceMax(),
      sort: this.sortOption(),
    });
  });

  protected readonly displayedCount = computed(() => this.displayedProducts().length);

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
  }

  protected clearFilters(): void {
    this.selectedCategoryIds.set(new Set());
    this.priceMax.set(null);
  }

  protected onPriceMaxInput(event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    if (raw === '') {
      this.priceMax.set(null);
      return;
    }
    const n = Number(raw);
    this.priceMax.set(Number.isFinite(n) ? n : null);
  }

  protected hasActiveFilters(): boolean {
    return this.selectedCategoryIds().size > 0 || this.priceMax() != null;
  }

  protected setSort(value: string): void {
    const v = value as ShopSortOption;
    if (v === 'name-asc' || v === 'name-desc' || v === 'price-asc' || v === 'price-desc') {
      this.sortOption.set(v);
    }
  }
}
