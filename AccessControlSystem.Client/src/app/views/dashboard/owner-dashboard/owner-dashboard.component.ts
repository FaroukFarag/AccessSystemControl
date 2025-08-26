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
import { LanguageService } from '../../../services/language/language.service';

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
    unit: undefined,
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
  sites: any[] = [];
  vistorsDetails: any[] = [
    {
      id: 1,
      name: 'John Doe',
      email: 'john@example.com',
      mobile: '+1234567890',
      startDate: '2024-01-01',
      endDate: '2024-12-31'
    },
    {
      id: 2,
      name: 'Jane Smith',
      email: 'jane@example.com',
      mobile: '+0987654321',
      startDate: '2024-02-01',
      endDate: '2024-11-30'
    }
  ];
  devicesTraffic = [];
  ownerDetails: any;
  userId: any;
  devicesList: any[] = [];
  
  constructor(private http: HttpClient,
    private userService: UserService,
    private accessGroupService: AccessGroupService,
    private deviceService: DeviceService,
    private router: Router,
    private languageService: LanguageService,
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
    this.getSubscriptionDevices();
    this.getVisitorsDetails();
    this.getAllSites();
  }

  getAllSites() {
    this.accessGroupService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;

      else
        notify(this.languageService.translate('messages.error.getting_sites'), 'error', 2000);
    })
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
      unit: this.ownerDetails?.unit,
      subscriptionId: this.formModel.subscriptionId
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
          unit: this.ownerDetails?.unit,
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
        this.ownerDetails = data.resultData;
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
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

  getSubscriptionDevices() {
    this.userService.getAll('Devices/GetSubscriptionDevices').subscribe((data: any) => {
      this.devicesTraffic = data.resultData;
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
        console.log('Visitors Details length:', this.vistorsDetails.length);
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
  onSettingsClick = (data: any) => {
    console.log('Settings button clicked');
    console.log('Visitor data:', data);
    this.selectedVisitor = data;
    console.log('Selected visitor for settings:', this.selectedVisitor);
    console.log('Visitor ID:', this.selectedVisitor?.id);
    console.log('All visitor properties:', Object.keys(this.selectedVisitor || {}));
    
    // Set popup visibility
    this.pauseVisitPopupVisible = true;
    console.log('Pause popup visibility set to:', this.pauseVisitPopupVisible);
    
    // Force change detection
    setTimeout(() => {
      console.log('Pause popup visibility after timeout:', this.pauseVisitPopupVisible);
    }, 100);
  }

  // Cancel button click handler
  onCancelClick = (e: any) => {
    console.log('Cancel button clicked');
    console.log('Row data:', e.row.data);
    this.selectedVisitor = e.row.data;
    console.log('Selected visitor for cancel:', this.selectedVisitor);
    console.log('Visitor ID:', this.selectedVisitor?.id);
    console.log('All visitor properties:', Object.keys(this.selectedVisitor || {}));
    
    // Set popup visibility
    this.cancelVisitPopupVisible = true;
    console.log('Cancel popup visibility set to:', this.cancelVisitPopupVisible);
    
    // Force change detection
    setTimeout(() => {
      console.log('Cancel popup visibility after timeout:', this.cancelVisitPopupVisible);
    }, 100);
  }

  // Confirm pause visit
  confirmPauseVisit() {
    if (this.selectedVisitor) {
      // Make API call to delete the visitor
      console.log('Deleting visitor via pause button:', this.selectedVisitor);
      console.log('Visitor ID being used:', this.selectedVisitor.id);
      console.log('All visitor properties:', Object.keys(this.selectedVisitor));
      
      this.baseService.delete(`Visitors/Delete/${this.selectedVisitor.id}`).subscribe({
        next: (response) => {
          console.log('Visitor deleted successfully via pause:', response);
          notify('Visitor deleted successfully', 'success', 2000);
          this.closePausePopup();
          this.getVisitorsDetails(); // Refresh the list
        },
        error: (error) => {
          console.error('Error deleting visitor via pause:', error);
          notify('Error deleting visitor: ' + (error.error?.message || error.message), 'error', 3000);
        }
      });
    }
  }

  // Confirm cancel visit
  confirmCancelVisit() {
    if (this.selectedVisitor) {
      // Make API call to delete the visitor
      console.log('Deleting visitor:', this.selectedVisitor);
      console.log('Visitor ID being used:', this.selectedVisitor.id);
      console.log('All visitor properties:', Object.keys(this.selectedVisitor));
      
      this.baseService.delete(`Visitors/Delete/${this.selectedVisitor.id}`).subscribe({
        next: (response) => {
          console.log('Visitor deleted successfully:', response);
          notify('Visitor deleted successfully', 'success', 2000);
          this.closeCancelPopup();
          this.getVisitorsDetails(); // Refresh the list
        },
        error: (error) => {
          console.error('Error deleting visitor:', error);
          notify('Error deleting visitor: ' + (error.error?.message || error.message), 'error', 3000);
        }
      });
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
