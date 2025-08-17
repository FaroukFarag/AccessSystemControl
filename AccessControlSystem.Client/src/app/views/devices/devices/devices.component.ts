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
import { DxDropDownButtonModule } from 'devextreme-angular/ui/drop-down-button';
import { DeviceService } from '../../../services/devices/device.service';
import { DomSanitizer } from '@angular/platform-browser';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { FormsModule } from '@angular/forms';
import { AccessGroupService } from '../../../services/access-groups/access-group.service';

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
  sortBy: string[] = [];
  accessGroupSortBy: string[] = [];

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
  fileUploaderKey = 0;
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
    private accessGroupService: AccessGroupService,) {
    this.deviceTypeEditorOptions = {
      dataSource: this.deviceTypes,
      valueExpr: 'id',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      placeholder: this.languageService.translate('devices.device_type_placeholder')
    };
  }

  ngOnInit() {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    // Initialize sort options with simple values
    this.sortBy = ['Recent', 'Name'];

    // Initialize sort options for access groups (limited options)
    this.accessGroupSortBy = ['Recent', 'Name'];

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
        notify(this.languageService.translate('messages.error.getting_sites'), 'error', 2000);
    })
  }

  getAllSchedules() {
    this.accessGroupService.getAll('AirfobSchedules/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.schedules = data.resultData.schedules;

      else
        notify(this.languageService.translate('messages.error.getting_schedules'), 'error', 2000);
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
    // Reset device data first
    this.deviceData = {
      deviceImageFile: null,
      deviceImageUrl: '',
      deviceName: '',
      deviceType: '',
      serial: '',
      siteId: null,
      macAddress: ''
    };

    // Reset validation error
    this.imageValidationError = '';

    // Show popup
    this.popupVisible = true;
  }

  onPopupHidden() {
    // Reset data when popup is closed
    this.deviceData = {
      deviceImageFile: null,
      deviceImageUrl: '',
      deviceName: '',
      deviceType: '',
      serial: '',
      siteId: null,
      macAddress: ''
    };
    this.imageValidationError = '';
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
      this.imageValidationError = this.languageService.translate('validation.image_required');
    }

    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    // Create FormData payload
    const formData = new FormData();

    // Append simple fields
    formData.append('Name', this.deviceData.deviceName);
    formData.append('deviceType', this.deviceData.deviceType);
    formData.append('Serial', this.deviceData.serial);
    formData.append('siteId', (this.deviceData.siteId || 0).toString());
    formData.append('MacAddress', this.deviceData.macAddress);
    formData.append('active', 'true'); // Convert boolean to string
    formData.append('subscriptionId', localStorage.getItem('subscriptionId')!);

    // Handle image file if present
    if (this.deviceData.deviceImageFile) {
      formData.append('imageFile', this.deviceData.deviceImageFile);
    }

    // Append imagePath if it exists
    if (this.deviceData.deviceImageUrl) {
      formData.append('imagePath', this.deviceData.deviceImageUrl);
    }

    // Now use this formData in your HTTP request

    this.deviceService.create('Devices/Create', formData as any).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify(this.languageService.translate('messages.success.device_created'), 'success', 1500);
          this.popupVisible = false;

          this.getAllDevices();
        } else {
          notify(response.message, 'error', 2000);
        }
      },
      error: (err) => {
        notify(this.languageService.translate('messages.error.device_creation'), 'error', 2000);
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
          notify(this.languageService.translate('messages.success.device_group_created'), 'success', 1500);

          this.groupDevice_popupVisible = false;
          this.groupName = '';
          this.selectedDevices = [];

          this.getAllDevices();
        } else {
          notify(this.languageService.translate('messages.error.device_group_creation'), 'error', 2000);
        }
      },
      error: (err) => {
        notify(this.languageService.translate('messages.error.device_group_creation'), 'error', 2000);
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
        notify(this.languageService.translate('messages.error.loading_access_groups'), 'error', 2000);
        console.error(err);
      }
    });
  }



  navigateToGroup(groupId: number) {
    // Example: navigate to group details page
    this.router.navigate(['/access-groups-devices'], { queryParams: { id: groupId } });

  }

  navigateToAssignedOwners(groupId: number) {
    // Navigate to owners page with group filter
    this.router.navigate(['/owners'], { queryParams: { groupId: groupId } });
  }

  onDevicesItemClick(e: any): void {
    const selectedSortOption = e.itemData;
    this.getAllDevices(selectedSortOption);
  }

  onAccessGroupItemClick(e: any): void {
    const selectedSortOption = e.itemData;
    this.getAllAccessGroups(selectedSortOption);
  }
}


