import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';

import { ButtonComponent } from './components/button/button.component';
import { CardComponent } from './components/card/card.component';
import { InputComponent } from './components/input/input.component';
import { ModalComponent } from './components/modal/modal.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { DebounceDirective } from './directives/debounce.directive';
import { CurrencyPipe } from './pipes/currency.pipe';

const COMPONENTS = [
  ButtonComponent,
  CardComponent,
  InputComponent,
  ModalComponent,
  SpinnerComponent,
];

@NgModule({
  declarations: [...COMPONENTS, CurrencyPipe, DebounceDirective],
  imports: [CommonModule, RouterModule, MatCardModule],
  exports: [CommonModule, RouterModule, ...COMPONENTS, CurrencyPipe, DebounceDirective],
})
export class SharedModule {}
