import { Routes } from '@angular/router';
import { DashboardComponent } from './views/dashboard/dashboard.component';
import { SubscriptionsComponent } from './views/subscriptions/subscriptions/subscriptions.component';
import { DevicesComponent } from './views/devices/devices/devices.component';
import { DeviceDetailsComponent } from './views/devices/device-details/device-details.component';
import { SubscriptionDetailsComponent } from './views/subscriptions/subscription-details/subscription-details.component';
import { OwnerDetailsComponent } from './views/owners/owner-details/owner-details.component';
import { OwnersComponent } from './views/owners/owners/owners.component';
import { UnitsComponent } from './views/units/units/units.component';
import { LoginComponent } from './views/login/login.component';
import { AccessGroupsComponent } from './views/access-groups/access-groups.component';
import { CardsComponent } from './views/cards/cards.component';
import { AccessGroupDevicesComponent } from './views/access-group-devices/access-group-devices.component'
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
    path: 'subscription-details/:id',
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
   {
     path: 'units',
     component: UnitsComponent
  },
  {
    path: 'unit-details',
    component: UnitsComponent
  },
  {
    path: 'access groups',
    component: AccessGroupsComponent
  }, {
    path: 'cards',
    component: CardsComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  { 
    path: 'access-groups-devices/:id',
    component: AccessGroupDevicesComponent 
  }
  //{
  //  path: 'dashboard',
  //  redirectTo: '/dashboard',
  //  pathMatch: 'full'
  //},
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
