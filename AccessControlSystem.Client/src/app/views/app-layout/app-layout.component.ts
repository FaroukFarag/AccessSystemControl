import { Component, OnInit } from '@angular/core';
import { ContentComponent } from './content/content.component';
import { FooterComponent } from './footer/footer.component';
import { DxDrawerModule, DxDrawerTypes } from 'devextreme-angular/ui/drawer';
import { DxListModule, DxToolbarModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-app-layout',
  imports: [
    DxDrawerModule,
    ContentComponent,
    FooterComponent,
    DxToolbarModule,
    DxListModule,
    CommonModule
  ],
  templateUrl: './app-layout.component.html',
  styleUrl: './app-layout.component.scss'
})
export class AppLayoutComponent implements OnInit {
  isClosed = false;
  selectedOpenMode: DxDrawerTypes.OpenedStateMode = 'shrink';
  selectedPosition: DxDrawerTypes.PanelLocation = 'left';
  selectedRevealMode: DxDrawerTypes.RevealMode = 'slide';
  isDrawerOpen = true;
  navigation: any = [];

  toggleSidebar() {
    this.isClosed = !this.isClosed;
  }

  initializeNavigation() {
    this.navigation = [
      { id: 1, text: 'Dashboard', icon: '/assets/icons/dashboard.svg' }
    ];
  }

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
      icon: 'login',
      stylingMode: 'text',
      text: 'Log out',
      onClick: () => console.log('LoggedOut'),
    },
  }];

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.initializeNavigation();
    }, 1000);

  }
}
