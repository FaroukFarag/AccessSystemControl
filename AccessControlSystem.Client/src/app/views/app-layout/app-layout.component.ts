import { Component, OnInit } from '@angular/core';
import { ContentComponent } from './content/content.component';
import { Router } from '@angular/router';
import { DxDrawerModule, DxDrawerTypes } from 'devextreme-angular/ui/drawer';
import { DxListModule, DxToolbarModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import { AppComponent } from './../../app.component';
@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [
    DxDrawerModule,
    ContentComponent,
    DxToolbarModule,
    DxListModule,
    CommonModule,
    AppComponent
  ],
  templateUrl: './app-layout.component.html',
  styleUrl: './app-layout.component.scss'
})
export class AppLayoutComponent implements OnInit {
  selectedOpenMode: DxDrawerTypes.OpenedStateMode = 'shrink';
  canAccessMainLayout: boolean = false;
  isFirstLogin: boolean = false;
  isClosed = false;
  selectedPosition: DxDrawerTypes.PanelLocation = 'left';
  selectedRevealMode: DxDrawerTypes.RevealMode = 'slide';
  isDrawerOpen = true;

  navigation: any = [
    { id: 1, text: 'Dashboard', icon: '/assets/icons/dashboard.svg' },
    { id: 2, text: 'Subscriptions', icon: '/assets/icons/subscriptions.svg' },
    { id: 3, text: 'Devices', icon: '/assets/icons/device.svg' },
    { id: 4, text: 'Owners', icon: '/assets/icons/owners.svg' },
    { id: 7, text: 'Admins', icon: '/assets/icons/owners.svg' },
    { id: 5, text: 'Units', icon: '/assets/icons/units.svg' },
    { id: 6, text: 'Access groups', icon: '/assets/icons/access.svg' },
    { id: 7, text: 'Cards', icon: '/assets/icons/card-owner.svg' },
  ];

  constructor(private router: Router) { }
  toolbarContent = [{
    widget: 'dxButton',
    location: 'before',
    options: {
      icon: 'menu',
      stylingMode: 'text',
      onClick: () => this.isDrawerOpen = !this.isDrawerOpen,
    },
  },
    {
      widget: 'dxButton',
      location: 'after',
      options: {
        icon: 'runner',
        text: 'Logout',
        stylingMode: 'text',
        onClick: () => this.logout(),
      }
    }
    //{
    //  widget: 'dxButton',
    //  location: 'after',
    //  options: {
    //    icon: 'login',
    //    stylingMode: 'text',
    //    text: 'Log out',
    //  },
    //},
  ];

  private getNavigationByRole(role: string): any[] {
    const allTabs = [
      { id: 1, text: 'Dashboard', icon: '/assets/icons/dashboard.svg' },
      { id: 2, text: 'Subscriptions', icon: '/assets/icons/subscriptions.svg' },
      { id: 3, text: 'Devices', icon: '/assets/icons/device.svg' },
      { id: 4, text: 'Owners', icon: '/assets/icons/owners.svg' },
      { id: 5, text: 'Admins', icon: '/assets/icons/owners.svg' },
      { id: 6, text: 'Units', icon: '/assets/icons/units.svg' },
      { id: 7, text: 'Access groups', icon: '/assets/icons/access.svg' },
      { id: 8, text: 'Cards', icon: '/assets/icons/card-owner.svg' },
    ];

    if (role === '1') {
      return allTabs.filter(tab => tab.id === 1 || tab.id === 2 || tab.id === 3);
    } else if (role === '2') {
      return allTabs.filter(tab => tab.id === 1 || tab.id === 2 || tab.id === 3 || tab.id === 6 || tab.id === 4 || tab.id === 7);
    }
    else {
      return allTabs.filter(tab => tab.id === 1 || tab.id === 3 );
    }
  }

  ngOnInit() {
    const role = localStorage.getItem('userRole') || '';
    const token = localStorage.getItem('authToken');

    this.canAccessMainLayout = !!token;
    this.navigation = this.getNavigationByRole(role);
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }

  toggleSidebar() {
    this.isClosed = !this.isClosed;
  }
  logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userRole');
    this.canAccessMainLayout = false;
    this.router.navigate(['/login']);
  }


}
