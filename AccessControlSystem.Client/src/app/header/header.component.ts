import { Component, OnInit, Output, EventEmitter, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SidebarService } from '../services/sidebar/sidebar.service';
import { LanguageService } from '../services/language/language.service';
import { TranslatePipe } from '../pipes/translate.pipe';
import { JwtService } from '../services/jwt.service';

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

  // User information from token
  currentUser: { userName: string; email: string; role: string; userId: string } | null = null;

  languages = [
    { code: 'en', name: 'English', flag: '🇺🇸' },
    { code: 'ar', name: 'العربية', flag: '🇸🇦' }
  ];

  direction: 'ltr' | 'rtl' = 'ltr';

  constructor(
    private router: Router,
    private sidebarService: SidebarService,
    private languageService: LanguageService,
    private jwtService: JwtService
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

    // Get current user from token
    this.loadCurrentUser();
  }

  /**
   * Loads the current user information from the JWT token
   */
  loadCurrentUser(): void {
    this.currentUser = this.jwtService.getCurrentUser();
    
    // Check if token is expired
    if (this.jwtService.isTokenExpired()) {
      this.logout();
    }
  }

  /**
   * Gets the user role translation key based on the role from token
   */
  getUserRoleTranslationKey(): string {
    if (!this.currentUser) {
      return 'user.user';
    }

    switch (this.currentUser.role.toLowerCase()) {
      case 'admin':
        return 'user.admin';
      case 'owner':
        return 'user.owner';
      case 'sub_admin':
        return 'user.sub_admin';
      default:
        return 'user.user';
    }
  }

  /**
   * Gets the user email for display or other purposes
   */
  getUserEmail(): string {
    return this.currentUser?.email || '';
  }

  /**
   * Gets the user ID for display or other purposes
   */
  getUserId(): string {
    return this.currentUser?.userId || '';
  }

  toggleSidebar(): void {
    this.sidebarService.toggle();
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

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userRole');
    localStorage.removeItem('subscriptionId');
    this.currentUser = null;
    this.loggedOut.emit();
    this.router.navigate(['/login']);
  }
}
