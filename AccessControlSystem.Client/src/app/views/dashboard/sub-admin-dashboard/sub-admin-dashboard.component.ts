import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/users/user.service';
import notify from 'devextreme/ui/notify';
import { DomSanitizer } from '@angular/platform-browser';
import { AccessGroupService } from '../../../services/access-groups/access-group.service';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import { AccessGroup } from '../../../models/access-group/access-group';
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';

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
  DxFormComponent,

} from 'devextreme-angular';
import { DeviceService } from '../../../services/devices/device.service';
import { UnitService } from '../../../services/units/unit.service';

@Component({
  selector: 'sub-admin-dashboard',
  standalone: true,
  imports: [CommonModule,
    TranslatePipe,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxFileUploaderModule,
    DxDropDownButtonModule,
    DxDataGridModule],
  templateUrl: './sub-admin-dashboard.component.html',
  styleUrl: './sub-admin-dashboard.component.scss'
})
export class SubAdminDashboardComponent {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  devicePopupVisible: boolean = false;
  unitPopupVisible: boolean = false;
  devicesList: any;
  imageValidationError: string = '';
  devicesCount!: number;

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

  unitsData = {
    unitImageFile: null,
    unitImageUrl: '',
    Name: '',
    Number: '',
    Area: '',
    CardNumber: '',
    AccessGroups: [],
    ImageEncode: '',
    ImageFile: null,
    ImagePath: '',
    Id: '0',
    UserId: '1',
    SubscriptionId: '0',
  };

  sites: any;
  subscriptionId: any;
  unitsList: any;
  ownersSortingList = ['Recent', 'Name'];
  unitsSortingList = ['Recent', 'Name'];
  ownersList: any;
  accessGroups: AccessGroup[] = [];
  dataSource: any[] = [

    {

      "Traffic type": "Check In",

      "Time": "12:00 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5E",

      "image": "path/to/image1.png"

    },

    {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },


  ];
  constructor(private router : Router,
    private deviceService: DeviceService,
    private accessGroupService: AccessGroupService,
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private unitsService: UnitService,

) {

    this.deviceTypeEditorOptions = {
      dataSource: this.deviceTypes,
      valueExpr: 'id',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      placeholder: 'Device type'
    };  }


  ngOnInit() {
    this.getAllDevices();
    this.getAllSites();
    this.getAllUnits();
    this.getAllOwners();
    this.subscriptionId = localStorage.getItem('subscriptionId');
    this.getAllAccessGroups();
    this.getDevicesCount();
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

  navigateToDevices() {
    this.router.navigate(['/devices']);
  }
  
  navigateToAddDevice() {
    this.router.navigate(['/devices'], { queryParams: { action: 'add' } });
  }
  
  navigateToDevicePage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }


  getAllSites() {
    this.accessGroupService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;

      else
        notify('Error getting sites', 'error', 2000);
    })
  }





  showAddDevicePopup() {
    this.devicePopupVisible = true;

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

    const formData = new FormData();
    formData.append('ImageFile', this.deviceData.deviceImageFile || '');
    formData.append('ImagePath', this.deviceData.deviceImageUrl || '');
    formData.append('Name', this.deviceData.deviceName);
    formData.append('DeviceType', this.deviceData.deviceType);
    formData.append('Serial', this.deviceData.serial);
    formData.append('SiteId', this.deviceData.siteId || '0');
    formData.append('MacAddress', this.deviceData.macAddress);
    formData.append('Active', 'true');
    formData.append('SubscriptionId', localStorage.getItem('subscriptionId')!);

    this.deviceService.create('Devices/Create', formData as any).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify('Device created successfully', 'success', 1500);
          this.devicePopupVisible = false;

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

  /*Sorting Functions*/
  onItemClick_OwnerSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllOwners(e.itemData);
  }

  onItemClick_unitsSorting(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllUnits(e.itemData);
  }


  navigateToUnits() {
    this.router.navigate(['/units']);
  }

  navigateToOwners() {
    this.router.navigate(['/owners']);

  }


  showAddUnitPopup() {
    this.subscriptionId = localStorage.getItem('subscriptionId');
    this.unitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroups: [],
      ImageEncode: '',
      ImageFile: null,
      ImagePath: '',
      Id: '0',
      UserId: '1',
      SubscriptionId: this.subscriptionId
    };
    this.imageValidationError = '';
    this.unitPopupVisible = true;
  }

  navigateToDetailsPage(unitId: number) {
    // this.router.navigate(['/unit-details', { id: unitId }]);
    this.router.navigate(['/unit-details'], { queryParams: { id: unitId } });

  }

  sanitizeImageUnits(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChangeUnits(e: any) {
    const file = e.value[0];
    if (file) {
      this.unitsData.unitImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.unitsData.unitImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  getSelectedAccessGroups(selectedAccessGroupIds: number[]): AccessGroup[] {
    return this.accessGroups.filter(ag =>
      selectedAccessGroupIds.includes(ag.id)
    );
  }

  submitInits() {
    // Validate required fields
    if (!this.unitsData.Name || !this.unitsData.Number || !this.unitsData.Area || !this.unitsData.CardNumber || !this.unitsData.SubscriptionId || !this.unitsData.AccessGroups || !this.unitsData.AccessGroups.length) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const formData = new FormData();
    const t = this.getSelectedAccessGroups(this.unitsData.AccessGroups);

    formData.append('name', this.unitsData.Name);
    formData.append('number', this.unitsData.Number.toString());
    formData.append('area', this.unitsData.Area.toString());
    formData.append('cardNumber', this.unitsData.CardNumber.toString());
    formData.append('subscriptionId', this.unitsData.SubscriptionId.toString());
    formData.append('AccessGroupsJson', JSON.stringify(t));

    // Check if an image file is selected
    if (this.unitsData.unitImageFile) {
      formData.append('imageFile', this.unitsData.unitImageFile);
    }


    if (this.unitsData.unitImageUrl) {
      formData.append('imagePath', this.unitsData.unitImageUrl);
    }


    console.log('Data being sent to the API:', {
      name: this.unitsData.Name,
      number: this.unitsData.Number,
      area: this.unitsData.Area,
      cardNumber: this.unitsData.CardNumber,
      subscriptionId: this.unitsData.SubscriptionId,
      accessGroups: this.unitsData.AccessGroups,
      imagePath: this.unitsData.unitImageUrl,
      imageFile: this.unitsData.unitImageFile ? this.unitsData.unitImageFile : null
    });


    this.unitsService.create('Units/Create', formData as any).subscribe({
      next: (response) => {
        notify('Unit created successfully', 'success', 1500);
        this.unitPopupVisible = false;
        this.getAllUnits();
      },
      error: (err) => {
        notify('Error creating Unit: ' + (err?.error?.details || err.message), 'error', 2000);
      }
    });
  }

  /*Sorting Function */

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllUnits(e.itemData);
  }

  getAllAccessGroups() {
    this.unitsService.getAll('AccessGroups/GetAll').subscribe((data: any) => {
      this.accessGroups = data.resultData;

    })
  }
  getDevicesCount() {
    this.userService.getAll('Devices/GetDevicesCount').subscribe((data: any) => {
      this.devicesCount = data.resultData;
    })
  }

}
