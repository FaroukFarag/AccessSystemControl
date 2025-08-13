import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface Translation {
  [key: string]: string | Translation;
}

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private translations: { [key: string]: Translation } = {};

  private currentLangSubject = new BehaviorSubject<string>('en');
  currentLang$ = this.currentLangSubject.asObservable();

  private directionSubject = new BehaviorSubject<'ltr' | 'rtl'>('ltr');
  direction$ = this.directionSubject.asObservable();

  constructor(private http: HttpClient) {
    // Load all languages at startup
    this.loadTranslations('en');
    this.loadTranslations('ar');

    // Initialize from localStorage if available
    const savedLang = localStorage.getItem('language');
    if (savedLang) {
      this.setLanguage(savedLang);
    }
  }

  private loadTranslations(lang: string) {
    this.http.get<Translation>(`/assets/i18n/${lang}.json`).subscribe({
      next: (translations) => {
        this.translations[lang] = translations;
      },
      error: (error) => {
        console.error(`Error loading translations for ${lang}:`, error);
      }
    });
  }

  setLanguage(lang: string) {
    if (!this.translations[lang]) {
      this.loadTranslations(lang);
    }
    this.currentLangSubject.next(lang);
    this.directionSubject.next(lang === 'ar' ? 'rtl' : 'ltr');
    localStorage.setItem('language', lang);
    document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
    document.documentElement.lang = lang;
  }

  translate(key: string): string {
    const currentLang = this.currentLangSubject.value;
    const keys = key.split('.');
    let value: any = this.translations[currentLang];
    
    for (const k of keys) {
      if (!value) break;
      value = value[k];
    }

    return typeof value === 'string' ? value : key;
  }
}