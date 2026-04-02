import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'appCurrency', standalone: false })
export class CurrencyPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (value == null || value === '') return '';
    const n = typeof value === 'string' ? Number(value) : value;
    return Number.isFinite(n) ? n.toFixed(2) : '';
  }
}
