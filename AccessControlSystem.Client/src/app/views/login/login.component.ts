import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { LanguageService } from '../../services/language/language.service';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  rememberMe: boolean = false;
  isLanguageDropdownOpen = false;
  currentLanguage = 'en';
  direction: 'ltr' | 'rtl' = 'ltr';

  languages = [
    { code: 'en', name: 'English', flag: '🇺🇸' },
    { code: 'ar', name: 'العربية', flag: '🇸🇦' }
  ];

  constructor(
    private http: HttpClient, 
    private router: Router, 
    private loginService: LoginService,
    private languageService: LanguageService
  ) {
    // Initialize current language
    const savedLang = localStorage.getItem('language');
    if (savedLang) {
      this.currentLanguage = savedLang;
      this.direction = savedLang === 'ar' ? 'rtl' : 'ltr';
    }

    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });
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

  onSubmit() {
    const loginData = {
      userName: this.email,
      password: this.password,
    };

    this.loginService.login(loginData).subscribe(response => {
      if (response) {
        if (response.succeeded) {
          localStorage.setItem('userId', response.resultData.userId);
          localStorage.setItem('authToken', response.resultData.token);
          localStorage.setItem('userRole', response.resultData.roleId);
          localStorage.setItem('subscriptionId', response.resultData.subscriptionId);
          
          this.router.navigate(['/dashboard']);
        }

        else {
          notify(this.languageService.translate('messages.error.login_failed'), 'error', 2000);
        }

      } else {
        notify(this.languageService.translate('messages.error.login_no_response'), 'error', 2000);
        console.error('Login failed: No response');
      }
    });
  }
}
