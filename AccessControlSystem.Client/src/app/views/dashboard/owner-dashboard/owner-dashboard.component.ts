import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClient } from '@angular/common/http';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxFormModule,
  DxDataGridModule,
  DxTagBoxModule
} from 'devextreme-angular';
import { UserService } from '../../../services/users/user.service';
import notify from 'devextreme/ui/notify';
import { AccessGroupService } from '../../../services/access-groups/access-group.service';
import { DeviceService } from '../../../services/devices/device.service';
import { Router } from '@angular/router';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { BaseService } from '../../../services/shared/base-service.service';

@Component({
  selector: 'app-owner-dashboard',
  standalone: true,
  imports: [CommonModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxFormModule,
    DxPopupModule,
    DxDataGridModule,
    DxTagBoxModule,
    TranslatePipe],
  templateUrl: './owner-dashboard.component.html',
  styleUrl: './owner-dashboard.component.scss'
})
export class OwnerDashboardComponent implements OnInit {
  manageVistors_popupVisible = false;
  pauseVisitPopupVisible = false;
  cancelVisitPopupVisible = false;
  selectedVisitor: any = null;

  formModel = {
    name: '',
    mobile: '',
    siteId: 0,
    email: '',
    startDate: new Date(),
    endDate: new Date(),
    notes: '',
    accessGroupIds: [],
    unitId: 1, // Change this to the correct unitId from your app
    subscriptionId: 0
  };

  owner = {
    name: 'Ahmed Adly',
    subscription: 'Premium',
    devices: 3
  };

  devices = [
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' },
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' },
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' }
  ];
  vistorsDetails = [
    {
      id: 1,
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      id: 2,
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      id: 3,
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      id: 4,
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      id: 5,
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },
  ];
  devicesTraffic = [
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
  ownerDetails: any;
  userId: any;
  accessGroups: any;
  devicesList: any[] = [];
  constructor(private http: HttpClient,
    private userService: UserService,
    private accessGroupService: AccessGroupService,
    private deviceService: DeviceService,
    private router: Router,
    private baseService: BaseService<any>
) { }

  ngOnInit() {
    const subId = localStorage.getItem('subscriptionId');
    this.formModel.subscriptionId = subId ? +subId : 0;
    this.userId = localStorage.getItem('userId') || '';
    console.log('User ID:', this.userId);
    
    // Only call getOwnerDetails if userId exists
    if (this.userId) {
      this.getOwnerDetails(this.userId);
    } else {
      console.warn('User ID not found in localStorage');
    }
    
    this.getAllDevices();
    this.getVisitorsDetails();
    this.getAllAccessGroups();
  }

  openManageVistorsPopup() {
    this.manageVistors_popupVisible = true;
  }

  submitVistor() {
    const payload = {
      id: 0,
      name: this.formModel.name,
      mobile: this.formModel.mobile, // Changed back to mobile as API expects
      siteId: this.formModel.siteId,
      email: this.formModel.email,
      startDate: this.formModel.startDate,
      endDate: this.formModel.endDate,
      notes: this.formModel.notes,
      unitId: this.formModel.unitId,
      subscriptionId: this.formModel.subscriptionId,
      accessGroupIds: this.formModel.accessGroupIds // Use form model value
    };

    console.log('Submitting visitor payload:', payload);

    this.baseService.create('Visitors/Create', payload).subscribe({
      next: (res) => {
        this.manageVistors_popupVisible = false;
        notify('Visitor created successfully', 'success', 2000);
        // Reset form
        this.formModel = {
          name: '',
          mobile: '',
          siteId: 0,
          email: '',
          startDate: new Date(),
          endDate: new Date(),
          notes: '',
          accessGroupIds: [],
          unitId: this.formModel.unitId,
          subscriptionId: this.formModel.subscriptionId
        };
        // Refresh visitors list
        this.getVisitorsDetails();
      },
      error: (err) => {
        console.error('Error creating visitor:', err);
        console.error('Error response:', err.error);
        notify('Error creating visitor: ' + (err.error?.message || err.message), 'error', 3000);
      }
    });
  }


  getOwnerDetails(id: string) {
    this.userService.getById('Users/GetOwnerDetails', this.userId).subscribe({
      next: (data: any) => {
        this.ownerDetails = data;
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }

  getAllAccessGroups() {
    this.accessGroupService.getAll('AccessGroups/GetAll').subscribe({
      next: (data: any) => {
        this.accessGroups = data.resultData;
        console.log("Access Groups", this.accessGroups);
      },
      error: (err) => {
        notify('Failed to load access groups', 'error', 2000);
        console.error(err);
      }
    });
  }

  navigateToDevices() {
    this.router.navigate(['/devices']);
  }
  navigateToDevicePage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }
  getAllDevices(orderBy?: string) {
    const baseUrl = 'Devices/GetAll';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.deviceService.getAll(url).subscribe({
      next: (data: any) => {
        this.devicesList = data.resultData;
      },
      error: (err) => console.error("Failed to load owners:", err)
    })
  }

  getVisitorsDetails() {
    this.baseService.getAll('Visitors/GetAll').subscribe({
      next: (data: any) => {
        console.log('Raw visitors API response:', data);
        if (data && data.resultData) {
          this.vistorsDetails = data.resultData;
        } else if (Array.isArray(data)) {
          this.vistorsDetails = data;
        } else {
          this.vistorsDetails = [];
        }
        console.log('Visitors Details:', this.vistorsDetails);
      },
      error: (err) => {
        console.error('Error fetching visitors details:', err);
        console.error('Error response:', err.error);
        notify('Error fetching visitors details: ' + (err.error?.message || err.message), 'error', 3000);
        this.vistorsDetails = [];
      }
    });
  }

  // Settings button click handler
  onSettingsClick = (e: any) => {
    this.selectedVisitor = e.row.data;
    this.pauseVisitPopupVisible = true;
  }

  // Cancel button click handler
  onCancelClick = (e: any) => {
    this.selectedVisitor = e.row.data;
    this.cancelVisitPopupVisible = true;
  }

  // Confirm pause visit
  confirmPauseVisit() {
    if (this.selectedVisitor) {
      // Here you would typically make an API call to pause the visit
      console.log('Pausing visit for:', this.selectedVisitor);
      notify('dashboard.owner_dashboard.visit_paused', 'success', 2000);
      this.closePausePopup();
    }
  }

  // Confirm cancel visit
  confirmCancelVisit() {
    if (this.selectedVisitor) {
      // Here you would typically make an API call to cancel the visit
      console.log('Canceling visit for:', this.selectedVisitor);
      notify('dashboard.owner_dashboard.visit_canceled', 'success', 2000);
      this.closeCancelPopup();
    }
  }

  // Close pause popup
  closePausePopup() {
    this.pauseVisitPopupVisible = false;
    this.selectedVisitor = null;
  }

  // Close cancel popup
  closeCancelPopup() {
    this.cancelVisitPopupVisible = false;
    this.selectedVisitor = null;
  }
}
