import { Routes } from '@angular/router';
import { DevicesComponent } from './views/devices/devices.component';
import { UnitsComponent } from './views/units/units.component';
import { SubscriptionsComponent } from './views/subscriptions/subscriptions.component';

export const routes: Routes = [
    {
        path: 'subscriptions',
        component: SubscriptionsComponent
    },
    {
        path: 'devices',
        component: DevicesComponent
    },
    {
        path: 'units',
        component: UnitsComponent
    }
];
