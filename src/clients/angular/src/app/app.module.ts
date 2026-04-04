import { isDevMode, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EffectsModule } from '@ngrx/effects';
import { RouteReuseStrategy } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { CatalogRouteReuseStrategy } from './core/routing/catalog-route-reuse.strategy';
import { API_BASE_URL } from './core/tokens';
import { CartEffects } from './features/cart/store/cart.effects';
import { CART_FEATURE_KEY, cartReducer } from './features/cart/store/cart.reducer';
import { CatalogEffects } from './features/catalog/store/catalog.effects';
import { CATALOG_FEATURE_KEY, catalogReducer } from './features/catalog/store/catalog.reducer';
import { environment } from './environments/environment';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule,
    StoreModule.forRoot(
      {},
      {
        runtimeChecks: {
          strictStateImmutability: true,
          strictActionImmutability: true,
        },
      },
    ),
    EffectsModule.forRoot([]),
    StoreModule.forFeature(CATALOG_FEATURE_KEY, catalogReducer),
    StoreModule.forFeature(CART_FEATURE_KEY, cartReducer),
    EffectsModule.forFeature([CatalogEffects, CartEffects]),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: !isDevMode(),
    }),
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useFactory: (): string => environment.apiBaseUrl.replace(/\/$/, ''),
    },
    { provide: RouteReuseStrategy, useClass: CatalogRouteReuseStrategy },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
