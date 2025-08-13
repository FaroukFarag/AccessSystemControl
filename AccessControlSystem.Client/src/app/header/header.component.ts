import { Component, OnInit, Output, EventEmitter, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SidebarService } from '../services/sidebar/sidebar.service';
import { LanguageService } from '../services/language/language.service';
import { TranslatePipe } from '../pipes/translate.pipe';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  isUserDropdownOpen = false;
  isLanguageDropdownOpen = false;
  currentLanguage = 'en';
  @Output() loggedOut = new EventEmitter<void>();

  languages = [
    { code: 'en', name: 'English', flag: '🇺🇸' },
    { code: 'ar', name: 'العربية', flag: '🇸🇦' }
    { code: 'de', name: 'Deutsch', flag: '🇩🇪' }
  ];

  direction: 'ltr' | 'rtl' = 'ltr';

  constructor(
    private router: Router,
    private sidebarService: SidebarService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    // Subscribe to language changes
    this.languageService.direction$.subscribe(dir => {
      this.direction = dir;
    });

    // Initialize current language
    const savedLang = localStorage.getItem('language');
    if (savedLang) {
      this.currentLanguage = savedLang;
      this.direction = savedLang === 'ar' ? 'rtl' : 'ltr';
    }
  }

  toggleSidebar(): void {
    this.sidebarService.toggle();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    const userDropdown = target.closest('.user-profile');
    const langDropdown = target.closest('.language-switcher');

    if (!userDropdown && this.isUserDropdownOpen) {
      this.isUserDropdownOpen = false;
    }

    if (!langDropdown && this.isLanguageDropdownOpen) {
      this.isLanguageDropdownOpen = false;
    }
  }

  toggleLanguageDropdown(): void {
    this.isLanguageDropdownOpen = !this.isLanguageDropdownOpen;
  }

  switchLanguage(langCode: string): void {
    this.currentLanguage = langCode;
    this.isLanguageDropdownOpen = false;
    this.languageService.setLanguage(langCode);
  }

  getCurrentLanguage() {
    return this.languages.find(lang => lang.code === this.currentLanguage);
  }

  toggleUserDropdown(): void {
    this.isUserDropdownOpen = !this.isUserDropdownOpen;
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userRole');
    this.loggedOut.emit();
    this.router.navigate(['/login']);
  }
}
