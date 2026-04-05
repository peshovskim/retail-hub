import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import type { ApiErrorBody } from '../../../core/models/auth.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register.page.html',
  styleUrl: './register.page.scss',
  standalone: false,
})
export class RegisterPage {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly submitting = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    email: [''],
    password: [''],
    confirmPassword: [''],
  });

  protected submit(): void {
    this.errorMessage.set(null);
    const { email, password, confirmPassword } = this.form.getRawValue();
    const trimmedEmail = email.trim();
    if (!trimmedEmail || !password) {
      this.errorMessage.set('Email and password are required.');
      return;
    }
    if (password !== confirmPassword) {
      this.errorMessage.set('Passwords do not match.');
      return;
    }

    this.submitting.set(true);
    this.auth.register(trimmedEmail, password).subscribe({
      next: () => {
        this.submitting.set(false);
        void this.router.navigateByUrl('/catalog');
      },
      error: (err: HttpErrorResponse) => {
        this.submitting.set(false);
        const body = err.error as ApiErrorBody | null;
        this.errorMessage.set(body?.message ?? 'Registration failed. Try again.');
      },
    });
  }
}
