import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Router } from '@angular/router';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxFormModule,
  DxFileUploaderModule,
} from 'devextreme-angular';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import { DeviceService } from '../../../services/devices/device.service';
import { DomSanitizer } from '@angular/platform-browser';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { FormsModule } from '@angular/forms';
import { AccessGroupService } from '../../../services/access-groups/access-group.service';
import { AccessGroup } from '../../../models/access-group/access-group'

@Component({
  selector: 'app-devices',
  standalone: true,
  imports: [CommonModule,
    FormsModule,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxDropDownButtonModule,
    DxFileUploaderModule,
    TranslatePipe
  ],
  templateUrl: './devices.component.html',
  styleUrl: './devices.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DevicesComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  selectedDevices: any = [];
  popupVisible: boolean = false;
  direction: 'ltr' | 'rtl' = 'ltr';
  groupDevice_popupVisible: boolean = false;
  sortBy = ['Recent', 'Name'];

  sites: any[] = [];
  schedules: any[] = [];
  devicesList: any[] = [];
  imageValidationError: string = '';
  deviceData = {
    deviceImageFile: null,
    deviceImageUrl: '',
    deviceName: '',
    deviceType: '',
    serial: '',
    siteId: null,
    macAddress: ''
  };
  macAddressPattern = /^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$/;
  deviceTypeEditorOptions: any;
  deviceTypes = [
    {
      'id': '1',
      'name': 'Airfob Edge Reader'
    },
    {
      'id': '2',
      'name': 'Airfob Edge Reader Ultimate'
    },
    {
      'id': '3',
      'name': 'Airfob Tag'
    },
    {
      'id': '4',
      'name': 'Airfob Patch'
    }, {
      'id': '5',
      'name': 'Suprema X-Station 2'
    }, {
      'id': '6',
      'name': 'Wireless Door Locks'
    },
  ];
  userRole: any;
  groupName: string = '';
  selectedSiteId!: number;
  selectedScheduleId: string = '';
  formSubmitted = false;
  accessGroups: any;

  constructor(private router: Router,
    private deviceService: DeviceService,
    private languageService: LanguageService,
    private sanitizer: DomSanitizer,
    private subscriptionsService: SubscriptionService,
    private accessGroupService: AccessGroupService,) {

    this.deviceTypeEditorOptions = {
      dataSource: this.deviceTypes,
      valueExpr: 'id',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      placeholder: 'Device type'
    };
  }

  ngOnInit() {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.getAllSites();
    this.getAllSchedules();
    this.getAllDevices();
    this.getAllAccessGroups();

    this.userRole = localStorage.getItem('userRole');
  }

  getAllSites() {
    this.accessGroupService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;

      else
        notify('Error getting sites', 'error', 2000);
    })
  }

  getAllSchedules() {
    this.accessGroupService.getAll('AirfobSchedules/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.schedules = data.resultData.schedules;

      else
        notify('Error getting schedules', 'error', 2000);
    })
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

  showAddDevicePopup() {
    this.popupVisible = true;
    
    this.deviceData = {
      deviceImageFile: null,
      deviceImageUrl: '',
      deviceName: '',
      deviceType: '',
      serial: '',
      siteId: null,
      macAddress: ''
    };
  }

  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }


  sanitizeImage(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChange(e: any) {
    const file = e.value[0];
    if (file) {
      this.deviceData.deviceImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.deviceData.deviceImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
  submitDevice() {
    this.imageValidationError = '';
    if (!this.deviceData.deviceImageFile) {
      this.imageValidationError = 'Image is required';
    }

    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    // Create JSON payload with subscription ID in request body
    const devicePayload = {
      imageFile: this.deviceData.deviceImageFile || null,
      imagePath: this.deviceData.deviceImageUrl || '',
      Name: this.deviceData.deviceName,
      deviceType: this.deviceData.deviceType,
      Serial: this.deviceData.serial,
      siteId: this.deviceData.siteId || 0,
      MacAddress: this.deviceData.macAddress,
      active: true,
      subscriptionId: +localStorage.getItem('subscriptionId')!
    };

    this.deviceService.create('Devices/Create', devicePayload as any).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify('Device created successfully', 'success', 1500);
          this.popupVisible = false;

          this.getAllDevices();
        } else {
          notify(response.errorMessage, 'error', 2000);
        }
      },
      error: (err) => {
        notify('Error creating device', 'error', 2000);
        console.error(err);
      }
    });
  }



  openGroupDEvicesPopup() {
    this.groupDevice_popupVisible = true;
    this.selectedDevices = [];
  }

  toggleDeviceSelection(device: any) {
    const index = this.selectedDevices.indexOf(device);

    if (index > -1) {
      this.selectedDevices.splice(index, 1);
    } else {
      this.selectedDevices.push(device);
    }

    console.log('Selected Device IDs:', this.selectedDevices);
  }

  submit() {
    const payload = {
      name: this.groupName,
      subscriptionId: +localStorage.getItem('subscriptionId')!,
      siteId: this.selectedSiteId,
      scheduleId: this.selectedScheduleId,
      devices: this.selectedDevices
    };

    this.accessGroupService.create('AccessGroups/Create', payload as any).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify('Device group created successfully', 'success', 1500);

          this.groupDevice_popupVisible = false;
          this.groupName = '';
          this.selectedDevices = [];

          this.getAllDevices();
        } else {
          notify('Failed to create device group', 'error', 2000);
        }
      },
      error: (err) => {
        notify('Failed to create device group', 'error', 2000);
        console.error(err);
      }
    });
  }

  getAllAccessGroups(orderBy?: string) {
    const baseUrl = 'AccessGroups/GetAll';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.accessGroupService.getAll(url).subscribe({
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



  navigateToGroup(groupId: number) {
    // Example: navigate to group details page
    this.router.navigate(['/access-groups-devices'], { queryParams: { id: groupId } });

  }

  onDevicesItemClick(e: any): void {
    this.getAllDevices(e.itemData);
  }

  onAccessGroupItemClick(e: any): void {
    this.getAllAccessGroups(e.itemData);
  }
}


