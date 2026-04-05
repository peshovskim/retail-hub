import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import type { ApiErrorBody } from '../../../core/models/auth.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss',
  standalone: false,
})
export class LoginPage {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  protected readonly submitting = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    email: [''],
    password: [''],
  });

  protected submit(): void {
    this.errorMessage.set(null);
    const { email, password } = this.form.getRawValue();
    const trimmedEmail = email.trim();
    if (!trimmedEmail || !password) {
      this.errorMessage.set('Enter your email and password.');
      return;
    }

    this.submitting.set(true);
    this.auth.login(trimmedEmail, password).subscribe({
      next: () => {
        this.submitting.set(false);
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] as string | undefined;
        void this.router.navigateByUrl(returnUrl && returnUrl.startsWith('/') ? returnUrl : '/catalog');
      },
      error: (err: HttpErrorResponse) => {
        this.submitting.set(false);
        const body = err.error as ApiErrorBody | null;
        this.errorMessage.set(body?.message ?? 'Sign in failed. Try again.');
      },
    });
  }
}
