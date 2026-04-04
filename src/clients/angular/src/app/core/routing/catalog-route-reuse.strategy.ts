import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouteReuseStrategy } from '@angular/router';

import { ProductListPage } from '../../features/catalog/pages/product-list.page';

/**
 * Angular’s default reuse compares route configs, so `/catalog` and `/catalog/category/:slug`
 * both use {@link ProductListPage} but still tear down the component — visible flash.
 * Reuse one list instance when both snapshots target that component.
 */
@Injectable()
export class CatalogRouteReuseStrategy extends RouteReuseStrategy {
  shouldDetach(): boolean {
    return false;
  }

  store(): void {}

  shouldAttach(): boolean {
    return false;
  }

  retrieve(): ActivatedRouteSnapshot | null {
    return null;
  }

  shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
    if (future.routeConfig === curr.routeConfig) {
      return true;
    }
    const next = future.routeConfig?.component;
    const prev = curr.routeConfig?.component;
    return next === ProductListPage && prev === ProductListPage;
  }
}
