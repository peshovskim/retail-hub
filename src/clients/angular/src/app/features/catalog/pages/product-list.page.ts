import { Component, computed, effect, inject, signal, untracked } from '@angular/core';
import { takeUntilDestroyed, toObservable, toSignal } from '@angular/core/rxjs-interop';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { distinctUntilChanged, map, skip } from 'rxjs';

import type { CategoryMenuNode } from '../models/category.model';
import type { ProductListParams, ProductListSort } from '../models/product-list.model';
import type { CatalogProductsView } from '../store/catalog.selectors';
import { CartFacade } from '../../cart/store/cart.facade';
import { CatalogToolbarSearchService } from '../services/catalog-toolbar-search.service';
import { CatalogFacade } from '../store/catalog.facade';

export type ShopSortOption = 'name-asc' | 'name-desc' | 'price-asc' | 'price-desc';

/** Default page size for catalog API requests (must stay ≤ API max, currently 100). */
const DEFAULT_CATALOG_PAGE_SIZE = 24;

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

function findCategoryNodeBySlug(nodes: CategoryMenuNode[], slug: string): CategoryMenuNode | null {
  for (const n of nodes) {
    if (n.slug === slug) {
      return n;
    }
    const child = findCategoryNodeBySlug(n.children ?? [], slug);
    if (child) {
      return child;
    }
  }
  return null;
}

/** All product category IDs under this node (leaf = self; parent = all descendant leaves). */
function categoryIdsForMenuNode(node: CategoryMenuNode): string[] {
  if (!node.children?.length) {
    return [node.id];
  }
  return node.children.flatMap(categoryIdsForMenuNode);
}

function setsEqualString(a: Set<string>, b: Set<string>): boolean {
  if (a.size !== b.size) {
    return false;
  }
  for (const x of a) {
    if (!b.has(x)) {
      return false;
    }
  }
  return true;
}

function shopSortOptionFromApiParam(value: string | null): ShopSortOption {
  switch (value) {
    case 'nameDesc':
      return 'name-desc';
    case 'priceAsc':
      return 'price-asc';
    case 'priceDesc':
      return 'price-desc';
    case 'nameAsc':
    default:
      return 'name-asc';
  }
}

/** Stable string for comparing query state (sorted category ids). */
function serializeCatalogQueryParams(record: Record<string, string>): string {
  const keys = Object.keys(record).sort();
  return keys.map((k) => `${k}=${record[k]}`).join('&');
}

function paramMapToRecord(qpm: ParamMap): Record<string, string> {
  const out: Record<string, string> = {};
  const search = qpm.get('search')?.trim();
  if (search) {
    out['search'] = search;
  }
  const cats = qpm.get('categoryIds')?.trim();
  if (cats) {
    out['categoryIds'] = cats
      .split(',')
      .map((s) => s.trim())
      .filter(Boolean)
      .sort()
      .join(',');
  }
  const priceMin = qpm.get('priceMin');
  if (priceMin != null && priceMin !== '') {
    out['priceMin'] = priceMin;
  }
  const priceMax = qpm.get('priceMax');
  if (priceMax != null && priceMax !== '') {
    out['priceMax'] = priceMax;
  }
  const sort = qpm.get('sort');
  if (sort != null && sort !== '' && sort !== 'nameAsc') {
    out['sort'] = sort;
  }
  const page = qpm.get('page');
  if (page != null && page !== '' && page !== '1') {
    out['page'] = page;
  }
  const pageSize = qpm.get('pageSize');
  if (pageSize != null && pageSize !== '' && pageSize !== String(DEFAULT_CATALOG_PAGE_SIZE)) {
    out['pageSize'] = pageSize;
  }
  return out;
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
  private readonly cart = inject(CartFacade);
  private readonly toolbarSearch = inject(CatalogToolbarSearchService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  /** `category/:slug` from the router (header Categories menu navigates here). */
  private readonly routeCategorySlug = toSignal(
    this.route.paramMap.pipe(map((pm) => pm.get('slug'))),
    { initialValue: this.route.snapshot.paramMap.get('slug') },
  );

  /** True while URL is `/catalog/category/:slug` so leaving that route clears the filter. */
  private readonly routeCategoryFilterActive = signal(false);

  /** Upper bound for the price range slider; values at the max mean “no max filter”. */
  protected readonly priceSliderCeiling = 5000;

  /** Mat range slider model (always numeric; maps to nullable filters at API boundaries). */
  sliderStart = 0;
  sliderEnd = this.priceSliderCeiling;

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

  constructor() {
    this.catalog.loadCategoryMenu();
    this.applyQueryParamsFromParamMap(this.route.snapshot.queryParamMap);
    this.reloadProducts({ skipUrlSync: true });

    this.route.queryParamMap
      .pipe(
        distinctUntilChanged(
          (a, b) => serializeCatalogQueryParams(paramMapToRecord(a)) === serializeCatalogQueryParams(paramMapToRecord(b)),
        ),
        skip(1),
        takeUntilDestroyed(),
      )
      .subscribe((qpm) => {
        if (
          serializeCatalogQueryParams(paramMapToRecord(qpm)) ===
          serializeCatalogQueryParams(this.buildQueryRecordFromState())
        ) {
          return;
        }
        this.applyQueryParamsFromParamMap(qpm);
        this.reloadProducts({ skipUrlSync: true });
      });

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

    toObservable(this.toolbarSearch.committed)
      .pipe(distinctUntilChanged(), skip(1), takeUntilDestroyed())
      .subscribe(() => {
        this.currentPage.set(1);
        this.reloadProducts();
      });

    effect(() => {
      const slug = this.routeCategorySlug();
      const nodes = this.menu();

      untracked(() => {
        if (slug) {
          if (nodes.length === 0) {
            return;
          }
          const node = findCategoryNodeBySlug(nodes, slug);
          if (!node) {
            return;
          }
          const ids = categoryIdsForMenuNode(node);
          if (ids.length === 0) {
            return;
          }
          const next = new Set(ids);
          if (setsEqualString(this.selectedCategoryIds(), next)) {
            this.routeCategoryFilterActive.set(true);
            return;
          }
          this.selectedCategoryIds.set(next);
          this.routeCategoryFilterActive.set(true);
          this.currentPage.set(1);
          this.reloadProducts();
          return;
        }

        if (this.routeCategoryFilterActive()) {
          this.routeCategoryFilterActive.set(false);
          this.selectedCategoryIds.set(new Set());
          this.currentPage.set(1);
          this.reloadProducts();
        }
      });
    });
  }

  private buildQueryRecordFromState(): Record<string, string> {
    const out: Record<string, string> = {};
    const search = this.toolbarSearch.committed().trim();
    if (search.length > 0) {
      out['search'] = search;
    }
    if (!this.route.snapshot.paramMap.get('slug')) {
      const ids = [...this.selectedCategoryIds()].filter(Boolean).sort();
      if (ids.length > 0) {
        out['categoryIds'] = ids.join(',');
      }
    }
    const pmin = this.priceMin();
    if (pmin != null && !Number.isNaN(pmin)) {
      out['priceMin'] = String(pmin);
    }
    const pmax = this.priceMax();
    if (pmax != null && !Number.isNaN(pmax)) {
      out['priceMax'] = String(pmax);
    }
    const sortApi = shopSortToApi(this.sortOption());
    if (sortApi !== 'nameAsc') {
      out['sort'] = sortApi;
    }
    const page = this.currentPage();
    if (page !== 1) {
      out['page'] = String(page);
    }
    const size = this.pageSize();
    if (size !== DEFAULT_CATALOG_PAGE_SIZE) {
      out['pageSize'] = String(size);
    }
    return out;
  }

  private syncQueryStringFromState(): void {
    const next = this.buildQueryRecordFromState();
    const current = paramMapToRecord(this.route.snapshot.queryParamMap);
    if (serializeCatalogQueryParams(next) === serializeCatalogQueryParams(current)) {
      return;
    }
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: next,
      replaceUrl: true,
      queryParamsHandling: '',
    });
  }

  private applyQueryParamsFromParamMap(qpm: ParamMap): void {
    const search = qpm.get('search')?.trim() ?? '';
    this.toolbarSearch.draft.set(search);
    this.toolbarSearch.committed.set(search);

    if (!this.route.snapshot.paramMap.get('slug')) {
      const raw = qpm.get('categoryIds')?.trim();
      const ids = raw ? raw.split(',').map((s) => s.trim()).filter((id) => id.length > 0) : [];
      this.selectedCategoryIds.set(new Set(ids));
    }

    const parseNum = (v: string | null): number | null => {
      if (v == null || v === '') {
        return null;
      }
      const n = Number(v);
      return Number.isFinite(n) && n >= 0 ? n : null;
    };
    const pmin = parseNum(qpm.get('priceMin'));
    const pmax = parseNum(qpm.get('priceMax'));
    this.priceMin.set(pmin);
    this.priceMax.set(pmax);
    const ceil = this.priceSliderCeiling;
    this.sliderStart = pmin != null && pmin > 0 ? pmin : 0;
    this.sliderEnd = pmax != null ? Math.min(pmax, ceil) : ceil;

    this.sortOption.set(shopSortOptionFromApiParam(qpm.get('sort')));

    const pageRaw = qpm.get('page');
    const page = pageRaw != null && pageRaw !== '' ? Number.parseInt(pageRaw, 10) : 1;
    this.currentPage.set(Number.isFinite(page) && page >= 1 ? page : 1);

    const sizeRaw = qpm.get('pageSize');
    const size = sizeRaw != null && sizeRaw !== '' ? Number.parseInt(sizeRaw, 10) : DEFAULT_CATALOG_PAGE_SIZE;
    this.pageSize.set(
      Number.isFinite(size) && size >= 1 && size <= 100 ? size : DEFAULT_CATALOG_PAGE_SIZE,
    );
  }

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
    this.sortOption.set('name-asc');
    this.pageSize.set(DEFAULT_CATALOG_PAGE_SIZE);
    this.currentPage.set(1);

    if (this.route.snapshot.paramMap.get('slug')) {
      void this.router.navigate(['/catalog'], {
        queryParams: {},
        replaceUrl: true,
        queryParamsHandling: '',
      });
      return;
    }

    this.reloadProducts();
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
      this.toolbarSearch.committed().trim().length > 0
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
    const search = this.toolbarSearch.committed().trim();
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

  private reloadProducts(options?: { skipUrlSync?: boolean }): void {
    this.catalog.loadProducts(this.buildParams());
    if (!options?.skipUrlSync) {
      this.syncQueryStringFromState();
    }
  }

  protected addOneToCart(event: Event, productId: string): void {
    event.preventDefault();
    event.stopPropagation();
    this.cart.addToCart(productId, 1);
  }
}
