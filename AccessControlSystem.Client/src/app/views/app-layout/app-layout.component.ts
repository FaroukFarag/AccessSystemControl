import { Component, OnInit } from '@angular/core';
import { ContentComponent } from './content/content.component';
import { Router } from '@angular/router';
import { DxDrawerModule, DxDrawerTypes } from 'devextreme-angular/ui/drawer';
import { DxListModule, DxToolbarModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common'; 
@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [
    DxDrawerModule,
    ContentComponent,
    DxToolbarModule,
    DxListModule,
    CommonModule
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
    { id: 5, text: 'Units', icon: '/assets/icons/dashboard.svg' }
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

  ngOnInit() { }
  navigateTo(route: string) {
    this.router.navigate([route]);
  }

  toggleSidebar() {
    this.isClosed = !this.isClosed;
  }
}
