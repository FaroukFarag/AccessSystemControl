import { Injectable } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { LoaderService } from '../services/loader/loader.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NavigationInterceptor {
  constructor(
    private router: Router,
    private loaderService: LoaderService
  ) {
    this.setupNavigationInterceptor();
  }

  private setupNavigationInterceptor(): void {
    // Show loader on navigation start
    this.router.events.pipe(
      filter(event => event instanceof NavigationStart)
    ).subscribe(() => {
      this.loaderService.show();
    });

    // Hide loader on navigation end
    this.router.events.pipe(
      filter(event => 
        event instanceof NavigationEnd || 
        event instanceof NavigationCancel || 
        event instanceof NavigationError
      )
    ).subscribe(() => {
      // Add a small delay to ensure smooth transition
      setTimeout(() => {
        this.loaderService.hide();
      }, 300);
    });
  }
}
