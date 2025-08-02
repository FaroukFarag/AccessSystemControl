import { Component } from '@angular/core';
import { DxDataGridModule, DxButtonModule, DxChartModule, DxSelectBoxModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common'
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';
import { UnitService } from '../../services/units/unit.service';
import { UserService } from '../../services/users/user.service';
import { OwnerDashboardComponent } from '../dashboard/owner-dashboard/owner-dashboard.component';
import { Router } from '@angular/router';
import { SubscriptionService } from '../../services/subscriptions/subscription.service';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    OwnerDashboardComponent,
    DxDataGridModule,
    DxButtonModule,
    DxChartModule,
    CommonModule, DxDropDownButtonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  subscriptionsCount!: number;
  devicesCount!: number;

  OwnersSortingList = [
    { text: 'Owner Name (A-Z)', value: 'usernameAsc' },
    { text: 'Owner Name (Z-A)', value: 'usernameDesc' },
    { text: 'Phone (Ascending)', value: 'phoneAsc' },
    { text: 'Phone (Descending)', value: 'phoneDesc' }
  ];

  UnitsSortingList = [
    { text: 'Name (A-Z)', value: 'nameAsc' },
    { text: 'Name (Z-A)', value: 'nameDesc' },
    { text: 'Devices Number (Low to High)', value: 'numberAsc' },
    { text: 'Devices Number (High to Low)', value: 'numberDesc' }
  ];

  subscriptionsSortingList = [
    { id: 'recent', name: 'Recent (Newest First)', text: 'Recent (Newest First)' },
    { id: 'oldest', name: 'Oldest First', text: 'Oldest First' },
    { id: 'customerNameAsc', name: 'Customer Name (A-Z)', text: 'Customer Name (A-Z)' },
    { id: 'customerNameDesc', name: 'Customer Name (Z-A)', text: 'Customer Name (Z-A)' }
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
    },
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    },
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    },
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    },
    {
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

  subscriptions: any;
  ownersList: any;
  unitsList: any;
  userRole: any;

  constructor(
    private unitsService: UnitService,
    private userService: UserService,
    private router: Router,
    private subscriptionsService: SubscriptionService) { }

  ngOnInit() {
    this.getSubscriptionsCount();
    this.getDevicesCount();

    this.userRole = localStorage.getItem('userRole');
    
    if (this.userRole === '1') {
      this.getAllSubscriptions();
    }
    
    this.getAllOwners();
    this.getAllUnits();
  }

  getSubscriptionsCount() {
    this.userService.getAll('Subscriptions/GetSubscriptionsCount').subscribe((data: any) => {
      this.subscriptionsCount = data.resultData;
    })
  }

  getDevicesCount() {
    this.userService.getAll('Devices/GetDevicesCount').subscribe((data: any) => {
      this.devicesCount = data.resultData;
    })
  }

  getAllOwners() {
    this.userService.getAll('Users/GetAllOwners').subscribe((data: any) => {
      this.ownersList = data.resultData;
      console.log("subscriptionssList", this.ownersList);
    })
  }

  getAllUnits() {
    this.unitsService.getAll('Units/GetAll').subscribe((data: any) => {
      this.unitsList = data.resultData;

    })
  }

  getAllSubscriptions() {
    this.subscriptionsService.getAll('Subscriptions/GetAll').subscribe((data: any) => {
      this.subscriptions = data.resultData;
      console.log("subscriptionssList", this.subscriptions);

    })
  }

  navigateToUnits() {
    this.router.navigate(['/units']);
  }

  navigateToOwners() {
    this.router.navigate(['/owners']);
  }

  navigateToSubscriptions() {
    this.router.navigate(['/subscriptions']);
  }

  /*Sorting Functions*/

  onItemClick_OwnerSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    const selected = e.itemData.value;

    switch (selected) {
      case 'usernameAsc':
        this.ownersList.sort((a: any, b: any) => a.userName.localeCompare(b.userName));
        break;
      case 'usernameDesc':
        this.ownersList.sort((a: any, b: any) => b.userName.localeCompare(a.userName));
        break;
      case 'phoneAsc':
        this.ownersList.sort((a: any, b: any) => a.phoneNumber.localeCompare(b.phoneNumber));
        break;
      case 'phoneDesc':
        this.ownersList.sort((a: any, b: any) => b.phoneNumber.localeCompare(a.phoneNumber));
        break;
    }

    notify(`Sorted by: ${e.itemData.text}`, 'success', 800);
  }

  onItemClick_unitsSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    const selected = e.itemData.value;
    switch (selected) {
      case 'nameAsc':
        this.unitsList.sort((a: any, b: any) => a.name.localeCompare(b.name));
        break;
      case 'nameDesc':
        this.unitsList.sort((a: any, b: any) => b.name.localeCompare(a.name));
        break;
      case 'numberAsc':
        this.unitsList.sort((a: any, b: any) => a.number - b.number);
        break;
      case 'numberDesc':
        this.unitsList.sort((a: any, b: any) => b.number - a.number);
        break;
    }
    notify(`Sorted by: ${e.itemData.name}`, 'success', 1000);
  }

  onItemClick_SubscriptionSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    const selected = e.itemData.id;

    switch (selected) {
      case 'recent':
        this.subscriptions.sort((a: any, b: any) => new Date(b.startDate).getTime() - new Date(a.startDate).getTime());
        break;
      case 'oldest':
        this.subscriptions.sort((a: any, b: any) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime());
        break;
      case 'customerNameAsc':
        this.subscriptions.sort((a: any, b: any) => a.customerName.localeCompare(b.customerName));
        break;
      case 'customerNameDesc':
        this.subscriptions.sort((a: any, b: any) => b.customerName.localeCompare(a.customerName));
        break;
    }

    notify(`Sorted by ${e.itemData.name}`, 'success', 600);
  }

}
