import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { filter } from 'rxjs/operators';
import { JwtService } from './services/jwt.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    HeaderComponent,
    SidebarComponent
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showLayout = true;

  constructor(
    public router: Router,
    private jwtService: JwtService
  ) {
    // Listen to route changes
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Check auth state after navigation
      this.checkAuthState();
    });
  }

  ngOnInit() {
    this.checkAuthState();
  }

  checkAuthState() {
    const token = localStorage.getItem('authToken');
    const isLoginPage = this.router.url.includes('/login');
    
    if (!token && !isLoginPage) {
      // No token and not on login page, redirect to login
      this.router.navigate(['/login']);
      this.showLayout = false;
    } else if (token && this.jwtService.isTokenExpired()) {
      // Token exists but is expired, clear storage and redirect to login
      localStorage.removeItem('authToken');
      localStorage.removeItem('userRole');
      localStorage.removeItem('subscriptionId');
      this.router.navigate(['/login']);
      this.showLayout = false;
    } else {
      // Normal case - valid token or on login page
      this.showLayout = !!token && !isLoginPage;
    }
  }

  onLogout() {
    this.showLayout = false;
  }
}
