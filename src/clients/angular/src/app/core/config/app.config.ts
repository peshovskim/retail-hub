import { environment } from '../../environments/environment';

/** Runtime app settings (not Angular `ApplicationConfig`). */
export const appSettings = {
  apiBaseUrl: environment.apiBaseUrl,
  production: environment.production,
} as const;
