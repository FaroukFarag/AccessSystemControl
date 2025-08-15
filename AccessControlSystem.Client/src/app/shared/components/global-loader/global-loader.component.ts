import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoaderService } from '../../../services/loader/loader.service';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';

@Component({
  selector: 'app-global-loader',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  template: `
    <div class="global-loading-overlay" *ngIf="isLoading">
      <div class="global-loader">
        <div class="global-spinner"></div>
        <p>{{ 'common.loading' | translate }}</p>
      </div>
    </div>
  `,
  styles: [`
    .global-loading-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(255, 255, 255, 0.9);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 99999;
      backdrop-filter: blur(2px);
    }

    .global-loader {
      text-align: center;
      background: white;
      padding: 2rem;
      border-radius: 12px;
      box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
    }

    .global-spinner {
      width: 60px;
      height: 60px;
      border: 4px solid #f3f3f3;
      border-top: 4px solid #007BE2;
      border-radius: 50%;
      animation: globalSpin 1s linear infinite;
      margin: 0 auto 20px;
    }

    .global-loader p {
      color: #666;
      font-size: 16px;
      margin: 0;
      font-weight: 500;
    }

    @keyframes globalSpin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
  `]
})
export class GlobalLoaderComponent implements OnInit {
  isLoading: boolean = false;

  constructor(
    private loaderService: LoaderService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loaderService.isLoading$.subscribe(isLoading => {
      this.isLoading = isLoading;
    });
  }
}
