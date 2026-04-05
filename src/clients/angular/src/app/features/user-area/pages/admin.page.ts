import { Component, inject } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin.page.html',
  styleUrl: './admin.page.scss',
  standalone: false,
})
export class AdminPage {
  protected readonly auth = inject(AuthService);
}
