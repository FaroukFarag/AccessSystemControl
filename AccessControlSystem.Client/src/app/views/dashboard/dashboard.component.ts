// dashboard.component.ts
import { Component } from '@angular/core';
import { DxDataGridModule, DxButtonModule, DxChartModule, DxSelectBoxModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common'
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DxDataGridModule,
    DxButtonModule,
    DxChartModule,
    CommonModule, DxDropDownButtonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  sortBy = ['Recent', 'date'];

  devicesData = [
    {
      subscriptionName: 'Device name',
      startDate: 'Sep 04, 2024',
      endDate: 'Sep 05, 2025',
      timeRemaining: '1 year and 2 months'
    },
    {
      subscriptionName: 'Device name',
      startDate: 'Sep 04, 2024',
      endDate: 'Sep 05, 2025',
      timeRemaining: '2 months'
    },
    {
      subscriptionName: 'Device name',
      startDate: 'Sep 04, 2024',
      endDate: 'Sep 05, 2025',
      timeRemaining: '4 months'
    }
  ];

  salesData = [
    { month: 'Jan', sales: 10000 },
    { month: 'Feb', sales: 12000 },
    { month: 'Mar', sales: 15000 },
    { month: 'Apr', sales: 13000 },
    { month: 'May', sales: 17000 },
    { month: 'Jun', sales: 20000 },
  ];

  devices = [
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    },
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '4 months'
    }
  ];

  subscriptions = [
    {
      name: 'Sunny Beach',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Standard',
      devices: 24,
    },
    {
      name: 'Ocean View',
      image: '/assets/images/beach.jpg',
      renewal: '4 months',
      type: 'Premium',
      devices: 24,
    },
    {
      name: 'Child’s Paradise',
      image: '/assets/images/beach.jpg',
      renewal: '1 year',
      type: 'Standard',
      devices: 24,
    },
    {
      name: 'Crystal Cove',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Premium',
      devices: 24,
    },{
      name: 'Crystal Cove',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Premium',
      devices: 24,
    },{
      name: 'Crystal Cove',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Premium',
      devices: 24,
    },{
      name: 'Crystal Cove',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Premium',
      devices: 24,
    },{
      name: 'Crystal Cove',
      image: '/assets/images/beach.jpg',
      renewal: '2 months',
      type: 'Premium',
      devices: 24,
    },
  ];

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
}
