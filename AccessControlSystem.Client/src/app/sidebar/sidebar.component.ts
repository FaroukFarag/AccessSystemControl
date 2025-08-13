import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarService } from '../services/sidebar/sidebar.service';
import { Router } from '@angular/router';
import { TranslatePipe } from '../pipes/translate.pipe';

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

  // Admin menu items (role 1)
  adminMenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', active: true },
    { icon: 'subscriptions', translationKey: 'navigation.subscriptions', active: false },
    { icon: 'device', translationKey: 'navigation.devices', active: false },
  ];

  // Owner menu items (role 2)
  ownerMenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', active: true },
    { icon: 'owners', translationKey: 'navigation.owners', active: false },
    { icon: 'device', translationKey: 'navigation.devices', active: false },
  ];

  // Owner menu items (role 3) - same as role 2
  ownerRole3MenuItems = [
    { icon: 'dashboard', translationKey: 'navigation.dashboard', active: true },
    { icon: 'device', translationKey: 'navigation.devices', active: false },
  ];

  constructor(
    private sidebarService: SidebarService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.sidebarService.isOpen$.subscribe(isOpen => {
      this.isOpen = isOpen;
    });
    
    // Get user role from localStorage
    this.userRole = parseInt(localStorage.getItem('userRole') || '0');
    
    // Set menu items based on user role
    this.setMenuItemsByRole();
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

  selectMenuItem(index: number): void {
    this.menuItems.forEach((item, i) => {
      item.active = i === index;
    });

    // Navigate based on selected menu item
    const selectedItem = this.menuItems[index];
    
    if (selectedItem.translationKey === 'navigation.subscriptions') {
      this.router.navigate(['/subscriptions']);
    } else if (selectedItem.translationKey === 'navigation.owners') {
      this.router.navigate(['/owners']);
    } else if (selectedItem.translationKey === 'navigation.devices') {
      this.router.navigate(['/devices']);
    } else {
      // Default to dashboard for any other case including 'dashboard'
      this.router.navigate(['/dashboard']);
    }
  }
}
