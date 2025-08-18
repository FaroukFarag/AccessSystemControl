import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { LoaderService } from '../../../services/loader/loader.service';

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
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';
import { DeviceService } from '../../../services/devices/device.service';

@Component({
  selector: 'app-subscription-details',
  standalone: true,
  imports: [CommonModule,
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
    DxDataGridModule,
    TranslatePipe],

  templateUrl: './subscription-details.component.html',
  styleUrl: './subscription-details.component.scss'
})
export class SubscriptionDetailsComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  @ViewChild('upgradeForm', { static: false }) upgradeForm!: DxFormComponent;
  id!: number;
  popupVisible: boolean = false;
  upgradePopupVisible: boolean = false;
  subscription: any;
  imageValidationError: string = '';
  deviceListEditorOptions: any;
  direction: 'ltr' | 'rtl' = 'ltr';
  isSidebarOpen: boolean = true;

  devicesList: any;
  deviceData = {
    deviceImageFile: null,
    deviceImageUrl: '',
    deviceName: '',
    deviceType: '',
    macAddress: '',
    serial: '',
    siteId: null
  };

  upgradeData = {
    id: 0,
    subscriptionType: 0,
    startDate: '',
    endDate: ''
  };

  isUpgrading: boolean = false;

  subscriptionTypes = [
    {
      'id': 1,
      'name': 'Standard'
    },
    {
      'id': 2,
      'name': 'Premium'
    },
    {
      'id': 3,
      'name': 'Enterprise'
    },
  ];
  macAddressPattern = /^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$/;
  sites: any[] = [];
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

  constructor(
    private route: ActivatedRoute,
    private subscriptionsService: SubscriptionService,
    private deviceService: DeviceService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private languageService: LanguageService,
    private sidebarService: SidebarService,
    private loaderService: LoaderService) {

    this.deviceListEditorOptions = {
      dataSource: this.devicesList,
      valueExpr: 'name',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      value: '',
      placeholder: 'Select Device'
    };
  }

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;

    this.loaderService.show(); // Start loading

    // Subscribe to sidebar state changes
    this.sidebarService.isOpen$.subscribe(isOpen => {
      this.isSidebarOpen = isOpen;
    });

    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.getAllSites();

    // Load subscription data using getAll and filter client-side
    this.subscriptionsService.getById('Subscriptions/Get', this.id).subscribe({
      next: (data: any) => {
        if (data && data.resultData) {
          const subscription = data.resultData;
          if (subscription) {
            this.subscription = subscription;
           
            // Load devices for this subscription after subscription is loaded
            this.getAllDevices();
          } else {
            notify(this.languageService.translate('validation.subscription_not_found'), 'error', 3000);
            this.router.navigate(['/subscriptions']);
          }
        } else {
          notify(this.languageService.translate('validation.subscription_not_found'), 'error', 3000);
          this.router.navigate(['/subscriptions']);
        }
      },
      error: (error) => {
        notify(this.languageService.translate('validation.error_loading_subscription_details'), 'error', 3000);
        this.router.navigate(['/subscriptions']);
      }
    });
  }


  getAllSites() {
    this.subscriptionsService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;
      else
        notify(this.languageService.translate('validation.sites_error'), 'error', 2000);
    })
  }

  getProgress(used: number, total: number): number {
    return (used / total) * 100;
  }

  getAllDevices() {
    this.deviceService.getAll(`Devices/GetAll`).subscribe((data: any) => {
      if (data && data.resultData) {
        const subscriptionDevices = data.resultData.filter((device: any) => device.subscriptionId === this.id);
        if (this.subscription) {
          this.subscription.devices = subscriptionDevices;
        }
        this.devicesList = subscriptionDevices;
      }
      this.loaderService.hide(); // Hide loader when devices are loaded
    })
  }

  showAddDevicePopup() {
    this.popupVisible = true;
    this.deviceData = {
      deviceImageFile: null,
      deviceImageUrl: '',
      deviceName: '',
      deviceType: '',
      macAddress: '',
      serial: '',
      siteId: null
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
      this.imageValidationError = this.languageService.translate('validation.image_required');
      return;
    }

    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    // Create JSON payload with subscription ID in request body
    const devicePayload = {
      imageFile: this.deviceData.deviceImageFile,
      imagePath: this.deviceData.deviceImageUrl || '',
      name: this.deviceData.deviceName,
      macAddress: this.deviceData.macAddress,
      deviceType: String(this.deviceData.deviceType),
      serial: this.deviceData.serial,
      siteId: this.deviceData.siteId || 0,
      active: true,
      subscriptionId: +this.id
    };

    this.deviceService.create('Devices/Create', devicePayload as any).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify(this.languageService.translate('validation.device_created'), 'success', 1500);
          this.popupVisible = false;

          this.getAllDevices();
        } else {
           notify(response.message, 'error', 2000);
        }
      },
      error: (err) => {
        notify(this.languageService.translate('validation.device_creation_error'), 'error', 2000);
        console.error(err);
      }
    });
  }


  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }
  getDeviceTypeNameById(id: string | number): string {
    const type = this.deviceTypes.find(t => t.id == id);
    return type ? type.name : '';
  }

  showUpgradePopup() {
    this.upgradePopupVisible = true;
    this.upgradeData = {
      id: this.id,
      subscriptionType: 0,
      startDate: '',
      endDate: ''
    };
  }

  submitUpgrade() {
    const result = this.upgradeForm.instance.validate();
    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    this.isUpgrading = true;

    const start = new Date(this.upgradeData.startDate);
    const end = new Date(this.upgradeData.endDate);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      notify('Invalid start or end date', 'error', 2000);
      return;
    }

    const startFormatted = start.toISOString().split('T')[0];
    const endFormatted = end.toISOString().split('T')[0];

    this.upgradeData.startDate = startFormatted;
    this.upgradeData.endDate = endFormatted;

    this.subscriptionsService.upgradeSubscription('Subscriptions/UpgradeSubscription', this.upgradeData as any).subscribe({
      next: (response: any) => {
        console.log('API Response received, setting isUpgrading to false');
        this.isUpgrading = false;
        if (response.succeeded) {
          notify(this.languageService.translate('validation.subscription_updated'), 'success', 1500);
          this.upgradePopupVisible = false;
          
          // Refresh subscription data using getAll and filter
          this.subscriptionsService.getById('Subscriptions/Get', this.id).subscribe((data: any) => {
            if (data && data.resultData) {
              const subscription = data.resultData;
              if (subscription) {
                this.subscription = subscription;
              }
            }
          });
        } else {
          notify(response.message || this.languageService.translate('validation.subscription_update_error'), 'error', 2000);
        }
      },
      error: (err) => {
        console.log('API Error received, setting isUpgrading to false');
        this.isUpgrading = false;
        notify(this.languageService.translate('validation.subscription_update_error'), 'error', 2000);
        console.error(err);
      }
    });
  }

}
