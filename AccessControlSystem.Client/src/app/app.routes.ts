import { Routes } from '@angular/router';
import { DashboardComponent } from './views/dashboard/dashboard.component';
import { SubscriptionsComponent } from './views/subscriptions/subscriptions/subscriptions.component';
import { DevicesComponent } from './views/devices/devices/devices.component';
import { DeviceDetailsComponent } from './views/devices/device-details/device-details.component';
import { SubscriptionDetailsComponent } from './views/subscriptions/subscription-details/subscription-details.component';
import { OwnerDetailsComponent } from './views/owners/owner-details/owner-details.component';
import { OwnersComponent } from './views/owners/owners/owners.component';

export const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,

  },
  {
    path: 'subscriptions',
    component: SubscriptionsComponent,

  },
  {
    path: 'devices',
    component: DevicesComponent,

  },
  {
    path: 'device-details',
    component: DeviceDetailsComponent

  },
  {
    path: 'subscription-details',
    component: SubscriptionDetailsComponent
  },
  {
    path: 'owners',
    component: OwnersComponent
  },
  {
    path: 'owner-details',
    component: OwnerDetailsComponent
  },
 
  //{
  //  path: 'locations',
  //  redirectTo: '/locations',
  //  pathMatch: 'full'
  //}, 
  //{
  //  path: '**',
  //  redirectTo: '/locations'
  //}
];
