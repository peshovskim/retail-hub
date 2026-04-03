import { Component, computed, effect, inject, signal } from '@angular/core';
import { takeUntilDestroyed, toObservable, toSignal } from '@angular/core/rxjs-interop';
import { PageEvent } from '@angular/material/paginator';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs';

import type { CategoryMenuNode } from '../models/category.model';
import type { ProductListParams, ProductListSort } from '../models/product-list.model';
import type { CatalogProductsView } from '../store/catalog.selectors';
import { CatalogToolbarSearchService } from '../services/catalog-toolbar-search.service';
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

/** Default page size for catalog API requests (must stay ≤ API max, currently 100). */
const DEFAULT_CATALOG_PAGE_SIZE = 24;

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
  private readonly toolbarSearch = inject(CatalogToolbarSearchService);

  /** Upper bound for the price range slider; values at the max mean “no max filter”. */
  protected readonly priceSliderCeiling = 5000;

  /** Mat range slider model (always numeric; maps to nullable filters at API boundaries). */
  sliderStart = 0;
  sliderEnd = this.priceSliderCeiling;

  constructor() {
    this.catalog.loadCategoryMenu();
    this.reloadProducts();

    effect(() => {
      const pv = this.productsView();
      if (pv.kind !== 'ok' || pv.totalCount === 0) {
        return;
      }
      if (pv.items.length === 0 && this.currentPage() > 1) {
        this.currentPage.set(1);
        this.reloadProducts();
      }
    });

    toObservable(this.toolbarSearch.text)
      .pipe(
        debounceTime(350),
        distinctUntilChanged(),
        skip(1),
        takeUntilDestroyed(),
      )
      .subscribe(() => {
        this.currentPage.set(1);
        this.reloadProducts();
      });
  }

  protected readonly productsView = toSignal(this.catalog.productsView$, {
    initialValue: { kind: 'loading' } as CatalogProductsView,
  });

  protected readonly menu = toSignal(this.catalog.menu$, { initialValue: [] as CategoryMenuNode[] });

  protected readonly menuLoading = toSignal(this.catalog.menuLoading$, { initialValue: false });

  protected readonly sortOption = signal<ShopSortOption>('name-asc');
  protected readonly selectedCategoryIds = signal(new Set<string>());
  protected readonly priceMin = signal<number | null>(null);
  protected readonly priceMax = signal<number | null>(null);
  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(DEFAULT_CATALOG_PAGE_SIZE);

  protected readonly pageSizeOptions = [12, 24, 48] as const;

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
    this.goToFirstPageAndReload();
  }

  protected clearFilters(): void {
    this.selectedCategoryIds.set(new Set());
    this.clearPriceFilter(false);
    this.toolbarSearch.clear();
    this.goToFirstPageAndReload();
  }

  /** Clears only min/max price (and slider). @param reload when false, caller reloads (e.g. clear all). */
  protected clearPriceFilter(reload = true): void {
    this.priceMin.set(null);
    this.priceMax.set(null);
    this.sliderStart = 0;
    this.sliderEnd = this.priceSliderCeiling;
    if (reload) {
      this.goToFirstPageAndReload();
    }
  }

  protected onPriceMinChange(event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    if (raw === '') {
      this.priceMin.set(null);
      this.sliderStart = 0;
    } else {
      const n = Number(raw);
      if (Number.isFinite(n) && n >= 0) {
        this.priceMin.set(n);
        this.sliderStart = Math.min(n, this.sliderEnd);
      }
    }
    this.goToFirstPageAndReload();
  }

  protected onPriceMaxChange(event: Event): void {
    const raw = (event.target as HTMLInputElement).value;
    if (raw === '') {
      this.priceMax.set(null);
      this.sliderEnd = this.priceSliderCeiling;
    } else {
      const n = Number(raw);
      if (Number.isFinite(n) && n >= 0) {
        this.priceMax.set(n);
        this.sliderEnd = Math.max(Math.min(n, this.priceSliderCeiling), this.sliderStart);
      }
    }
    this.goToFirstPageAndReload();
  }

  protected onPriceSliderChange(): void {
    const low = this.sliderStart;
    const high = this.sliderEnd;
    this.priceMin.set(low <= 0 ? null : low);
    this.priceMax.set(high >= this.priceSliderCeiling ? null : high);
    this.goToFirstPageAndReload();
  }

  protected hasActiveFilters(): boolean {
    return (
      this.selectedCategoryIds().size > 0 ||
      this.priceMin() != null ||
      this.priceMax() != null ||
      this.toolbarSearch.text().trim().length > 0
    );
  }

  protected hasPriceFilter(): boolean {
    return this.priceMin() != null || this.priceMax() != null;
  }

  protected setSort(value: unknown): void {
    const v = String(value) as ShopSortOption;
    if (v === 'name-asc' || v === 'name-desc' || v === 'price-asc' || v === 'price-desc') {
      this.sortOption.set(v);
      this.goToFirstPageAndReload();
    }
  }

  protected onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.reloadProducts();
  }

  protected showPagination(totalCount: number): boolean {
    return totalCount > this.pageSize();
  }

  protected pageRange(totalCount: number): { start: number; end: number } {
    if (totalCount === 0) {
      return { start: 0, end: 0 };
    }
    const size = this.pageSize();
    const page = this.currentPage();
    const start = (page - 1) * size + 1;
    const end = Math.min(page * size, totalCount);
    return { start, end };
  }

  private goToFirstPageAndReload(): void {
    this.currentPage.set(1);
    this.reloadProducts();
  }

  private buildParams(): ProductListParams {
    const sort = shopSortToApi(this.sortOption());
    const categoryIds = [...this.selectedCategoryIds()];
    const priceMin = this.priceMin();
    const priceMax = this.priceMax();
    const search = this.toolbarSearch.text().trim();
    const params: ProductListParams = { sort };
    if (categoryIds.length > 0) {
      params.categoryIds = categoryIds;
    }
    if (priceMin != null && !Number.isNaN(priceMin) && priceMin >= 0) {
      params.priceMin = priceMin;
    }
    if (priceMax != null && !Number.isNaN(priceMax) && priceMax >= 0) {
      params.priceMax = priceMax;
    }
    if (search.length > 0) {
      params.search = search;
    }
    params.page = this.currentPage();
    params.pageSize = this.pageSize();
    return params;
  }

  private reloadProducts(): void {
    this.catalog.loadProducts(this.buildParams());
  }
}
