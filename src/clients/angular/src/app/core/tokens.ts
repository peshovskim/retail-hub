import { InjectionToken } from '@angular/core';

/**
 * Root API origin without a trailing slash.
 * Same idea as Zalary's `APP_CONFIGURATION` / API URL tokens: inject config instead of importing `environment` in every service (easier to test and override).
 */
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');
