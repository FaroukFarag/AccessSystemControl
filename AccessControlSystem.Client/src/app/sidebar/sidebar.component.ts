import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarService } from '../services/sidebar/sidebar.service';
import { Router, NavigationEnd } from '@angular/router';
import { TranslatePipe } from '../pipes/translate.pipe';
import { LanguageService } from '../services/language/language.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  isOpen = true;
  userRole: number = 0;
  menuItems: any[] = [];
  direction: 'ltr' | 'rtl' = 'ltr';

  // Admin menu items (role 1)
  adminMenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', route: '/dashboard', active: false },
    { icon: 'subscriptions', translationKey: 'navigation.subscriptions', route: '/subscriptions', active: false },
    { icon: 'device', translationKey: 'navigation.devices', route: '/devices', active: false },
  ];

  // Owner menu items (role 2)
  ownerMenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', route: '/dashboard', active: false },
    { icon: 'owners', translationKey: 'navigation.owners', route: '/owners', active: false },
    { icon: 'device', translationKey: 'navigation.devices', route: '/devices', active: false },
  ];

  // Owner menu items (role 3) - same as role 2
  ownerRole3MenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', route: '/dashboard', active: false },
    { icon: 'device', translationKey: 'navigation.devices', route: '/devices', active: false },
  ];

  constructor(
    private sidebarService: SidebarService,
    private router: Router,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.sidebarService.isOpen$.subscribe(isOpen => {
      this.isOpen = isOpen;
    });
    
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });
    
    // Subscribe to route changes to update active menu item
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.updateActiveMenuItem();
    });
    
    // Get user role from localStorage
    this.userRole = parseInt(localStorage.getItem('userRole') || '0');
    
    // Set menu items based on user role
    this.setMenuItemsByRole();
    
    // Set initial active menu item based on current route
    this.updateActiveMenuItem();
  }

  setMenuItemsByRole(): void {
    if (this.userRole === 2) {
      // Sub-Admin role - show Dashboard, Owners, Devices
      this.menuItems = [...this.ownerMenuItems];
    } else if (this.userRole === 3) {
      // Owner role - show Dashboard, Devices
      this.menuItems = [...this.ownerRole3MenuItems];
    } else {
      // Admin role (default) - show Dashboard, Subscriptions, Devices
      this.menuItems = [...this.adminMenuItems];
    }
  }

  /**
   * Updates the active menu item based on the current route
   */
  updateActiveMenuItem(): void {
    const currentUrl = this.router.url;
    
    // Reset all active states
    this.menuItems.forEach(item => {
      item.active = false;
    });
    
    // Find and set the active item based on current route
    const activeItem = this.menuItems.find(item => {
      if (item.route === '/dashboard') {
        return currentUrl === '/dashboard' || currentUrl === '/';
      }
      return currentUrl.startsWith(item.route);
    });
    
    if (activeItem) {
      activeItem.active = true;
    } else {
      // Default to dashboard if no match found
      const dashboardItem = this.menuItems.find(item => item.route === '/dashboard');
      if (dashboardItem) {
        dashboardItem.active = true;
      }
    }
  }

  selectMenuItem(index: number): void {
    // Reset all active states
    this.menuItems.forEach((item, i) => {
      item.active = i === index;
    });

    // Navigate based on selected menu item
    const selectedItem = this.menuItems[index];
    this.router.navigate([selectedItem.route]);
  }
}
