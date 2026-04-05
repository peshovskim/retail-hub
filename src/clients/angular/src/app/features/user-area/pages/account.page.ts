import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-account-page',
  templateUrl: './account.page.html',
  styleUrl: './account.page.scss',
  standalone: false,
})
export class AccountPage {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  protected readonly user = this.auth.user;

  constructor() {
    if (this.auth.user()) {
      this.loading.set(false);
      return;
    }
    this.auth.refreshUser().subscribe({
      next: () => this.loading.set(false),
      error: (err) => {
        this.loading.set(false);
        if (err instanceof HttpErrorResponse && err.status === 401) {
          void this.router.navigate(['/auth/login'], { queryParams: { returnUrl: this.router.url } });
          return;
        }
        this.error.set(
          'Could not load your profile. Start the API (e.g. https://localhost:7296) and ensure `environment.apiBaseUrl` matches it.',
        );
      },
    });
  }

  protected logout(): void {
    this.auth.logout();
    void this.router.navigateByUrl('/catalog');
  }
}
