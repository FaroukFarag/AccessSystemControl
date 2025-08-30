// src/app/shared/utils/date-util.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateUtilService {
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    
    return `${year}-${month}-${day}`;
  }

  formatDateForDisplay(date: Date, locale: string = 'en-US'): string {
    return date.toLocaleDateString(locale);
  }

  parseDate(dateString: string): Date {
    return new Date(dateString + 'T00:00:00');
  }

  getTodayFormat(): string {
    return this.formatDate(new Date());
  }

  isValidFormat(dateString: string): boolean {
    const regex = /^\d{4}-\d{2}-\d{2}$/;

    return regex.test(dateString);
  }
}