import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenu, MatMenuModule } from '@angular/material/menu';
import { RouterLink } from '@angular/router';

import type { CategoryMenuNode } from '../../models/category.model';

/**
 * Recursive Material menu for the category tree. Exposes `menu` for a parent
 * {@link MatMenuTrigger} (root trigger lives on {@link CategoryMenuComponent}).
 */
@Component({
  selector: 'app-category-nested-menu',
  standalone: true,
  imports: [
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    RouterLink,
    forwardRef(() => CategoryNestedMenuComponent),
  ],
  templateUrl: './category-nested-menu.component.html',
})
export class CategoryNestedMenuComponent {
  @Input({ required: true }) nodes!: CategoryMenuNode[];
  @Input() panelClass = '';

  @ViewChild('panel', { read: MatMenu }) menu!: MatMenu;
}
