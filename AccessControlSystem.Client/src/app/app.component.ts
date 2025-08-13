import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { filter } from 'rxjs/operators';

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

  constructor(public router: Router) {
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
    } else {
      // Normal case
      this.showLayout = !!token && !isLoginPage;
    }
  }

  onLogout() {
    this.showLayout = false;
  }
}
