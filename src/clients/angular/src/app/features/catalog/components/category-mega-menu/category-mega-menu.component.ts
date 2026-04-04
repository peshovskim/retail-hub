import { Component, Input, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatMenu, MatMenuModule } from '@angular/material/menu';
import { RouterLink, RouterLinkActive } from '@angular/router';

import type { CategoryMenuNode } from '../../models/category.model';

@Component({
  selector: 'app-category-mega-menu',
  standalone: true,
  imports: [MatMenuModule, RouterLink, RouterLinkActive],
  templateUrl: './category-mega-menu.component.html',
  styleUrl: './category-mega-menu.component.scss',
  /* Menu content is attached to CDK overlay outside the host; emulated encapsulation would not scope these rules to the portal. */
  encapsulation: ViewEncapsulation.None,
})
export class CategoryMegaMenuComponent {
  @Input({ required: true }) nodes!: CategoryMenuNode[];

  @ViewChild('panel', { read: MatMenu }) menu!: MatMenu;
}
