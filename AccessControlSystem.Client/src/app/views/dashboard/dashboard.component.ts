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
  ownersSortingList = ['Recent', 'Name'];
  unitsSortingList = ['Recent', 'Name'];
  subscriptionsSortingList = ['Recent', 'Name', 'Subscription'];
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

  getAllOwners(orderBy?: string): void {
    const baseUrl = 'Users/GetAllOwners';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.userService.getAll(url).subscribe({
      next: (data: any) => {
        this.ownersList = data.resultData;
      },
      error: (err) => console.error("Failed to load owners:", err)
    });
  }

  getAllUnits(orderBy?: string): void {
    const baseUrl = 'Units/GetAll';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.userService.getAll(url).subscribe({
      next: (data: any) => {
        this.unitsList = data.resultData;
      },
      error: (err) => console.error("Failed to load units:", err)
    });
  }

  getAllSubscriptions(orderBy?: string): void {
    const baseUrl = 'Subscriptions/GetAll';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.subscriptionsService.getAll(url).subscribe({
      next: (data: any) => {
        this.subscriptions = data.resultData;
        console.log("Subscriptions List:", this.subscriptions);
      },
      error: (err) => console.error("Failed to load subscriptions:", err)
    });
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
    this.getAllOwners(e.itemData);
  }

  onItemClick_unitsSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllUnits(e.itemData);
  }

  onItemClick_SubscriptionSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllSubscriptions(e.itemData);
  }

}
