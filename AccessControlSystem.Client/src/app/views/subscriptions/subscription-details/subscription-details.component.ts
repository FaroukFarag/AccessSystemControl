import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common'
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { LoaderService } from '../../../services/loader/loader.service';
import { UserService } from '../../../services/users/user.service';
import { trigger, state, style, transition, animate, query, stagger } from '@angular/animations';
import { BackButtonComponent } from '../../../shared/components/back-button/back-button.component';

import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxNumberBoxModule,
  DxFormModule,
  DxFileUploaderModule,
} from 'devextreme-angular';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';
import { DeviceService } from '../../../services/devices/device.service';
import { DateUtilService } from '../../../services/shared/utils/date-util.service';

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
    DxNumberBoxModule,
    DxFormModule,
    DxDropDownButtonModule,
    DxFileUploaderModule,
    DxDataGridModule,
    TranslatePipe,
    BackButtonComponent],

  templateUrl: './subscription-details.component.html',
  styleUrl: './subscription-details.component.scss',
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(30px)' }),
        animate('0.6s ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ]),
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('0.5s ease-out', style({ opacity: 1 }))
      ])
    ])
  ]
})
export class SubscriptionDetailsComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  @ViewChild('upgradeForm', { static: false }) upgradeForm!: DxFormComponent;
  @ViewChild('adminForm', { static: false }) adminForm!: DxFormComponent;
  id!: number;
  popupVisible: boolean = false;
  upgradePopupVisible: boolean = false;
  cancelConfirmationPopupVisible: boolean = false;
  isCancelling: boolean = false;
  subscription: any;
  imageValidationError: string = '';
  deviceListEditorOptions: any;
  direction: 'ltr' | 'rtl' = 'ltr';
  isSidebarOpen: boolean = true;
  subscriptionAdmins: any[] = [];

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
    monthNumber: 0
  };

  isUpgrading: boolean = false;

  // Admin popup properties
  addAdminPopupVisible: boolean = false;
  isAddingAdmin: boolean = false;

  // Admin form data
  adminData = {
    userName: '',
    email: '',
    password: '',
    phoneNumber: ''
  };

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
  dataSource!: any[];

  constructor(
    private route: ActivatedRoute,
    private subscriptionsService: SubscriptionService,
    private deviceService: DeviceService,
    private userService: UserService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private languageService: LanguageService,
    private sidebarService: SidebarService,
    private loaderService: LoaderService,
      private dateUtil: DateUtilService,
      private location: Location) {

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

    this.loaderService.show();

    // Subscribe to sidebar state changes
    this.sidebarService.isOpen$.subscribe(isOpen => {
      this.isSidebarOpen = isOpen;
    });

    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.getAllSites();
    this.getDevicesTraffic();
    this.getSubscriptionAdmins();

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

  getDevicesTraffic() {
    this.deviceService.getAll(`Devices/GetDevicesTraffic?subscriptionId=${this.id}`).subscribe((data: any) => {
      if (data.succeeded) {
        this.dataSource = data.resultData
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

  showAddAdminPopup() {
    this.addAdminPopupVisible = true;
    this.resetAdminData();
  }

  resetAdminData() {
    this.adminData = {
      userName: '',
      email: '',
      password: '',
      phoneNumber: ''
    };
  }



  submitAdmin() {
    const result = this.adminForm.instance.validate();
    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    this.isAddingAdmin = true;

    // Create admin payload
    const adminPayload = {
      userName: this.adminData.userName,
      email: this.adminData.email,
      password: this.adminData.password,
      phoneNumber: this.adminData.phoneNumber,
      roleId: 2,
      subscriptionId: this.id
    };

    this.userService.create('Users/Create', adminPayload as any).subscribe({
      next: (response: any) => {
        this.isAddingAdmin = false;
        if (response.succeeded) {
          notify(this.languageService.translate('validation.admin_created'), 'success', 1500);
          this.addAdminPopupVisible = false;
          this.getSubscriptionAdmins();
        } else {
          notify(response.message || this.languageService.translate('validation.admin_creation_error'), 'error', 2000);
        }
      },
      error: (err) => {
        this.isAddingAdmin = false;
        notify(this.languageService.translate('validation.admin_creation_error'), 'error', 2000);
        console.error(err);
      }
    });
  }

  getSubscriptionAdmins() {
    if (this.id) {
      this.userService.getAll(`Users/GetSubscriptionAdmins?subscriptionId=${this.id}`).subscribe({
        next: (response: any) => {
          if (response.succeeded) {
            this.subscriptionAdmins = response.resultData || [];
          } else {
            this.subscriptionAdmins = [];
          }
        },
        error: (err: any) => {
          console.error('Error fetching subscription admins:', err);
          this.subscriptionAdmins = [];
        }
      });
    }
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
      monthNumber: 0
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

    if (isNaN(start.getTime())) {
      notify('Invalid start date', 'error', 2000);
      return;
    }

    const startFormatted = this.dateUtil.formatDate(start);

    this.upgradeData.startDate = startFormatted;

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

  showCancelConfirmationPopup() {
    this.cancelConfirmationPopupVisible = true;
  }

  confirmCancelSubscription() {
    this.isCancelling = true;

    console.log('Attempting to cancel subscription with ID:', this.id);

    // First, try the standard DELETE approach
    this.tryDeleteEndpoints();
  }

  private tryDeleteEndpoints() {
    // Try different endpoint formats - first try with query parameter
    this.subscriptionsService.delete(`Subscriptions/Delete?id=${this.id}`).subscribe({
      next: (response: any) => {
        this.isCancelling = false;
        console.log('Delete response:', response);
        if (response.succeeded) {
          notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancelled_successfully'), 'success', 2000);
          this.cancelConfirmationPopupVisible = false;
          // Navigate back to subscriptions list
          this.router.navigate(['/subscriptions']);
        } else {
          notify(response.message || this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
        }
      },
      error: (err) => {
        this.isCancelling = false;
        console.error('Error cancelling subscription:', err);
        console.error('Error status:', err.status);
        console.error('Error status text:', err.statusText);

        // Log the full error details for debugging
        if (err.error) {
          console.error('Error details:', err.error);
        }

        // If first attempt fails, try alternative endpoint formats
        if (err.status === 400) {
          console.log('Trying alternative endpoint formats...');

          // Try different endpoint patterns
          const endpoints = [
            `Subscriptions/Delete/${this.id}`,
            `Subscriptions/${this.id}`,
            `Subscription/Delete?id=${this.id}`,
            `Subscription/${this.id}`
          ];

          let attemptCount = 0;
          const tryNextEndpoint = () => {
            if (attemptCount >= endpoints.length) {
              this.isCancelling = false;
              console.error('All endpoint attempts failed');
              notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
              return;
            }

            const endpoint = endpoints[attemptCount];
            console.log(`Trying endpoint: ${endpoint}`);

            this.subscriptionsService.delete(endpoint).subscribe({
              next: (response2: any) => {
                this.isCancelling = false;
                console.log('Alternative endpoint response:', response2);
                if (response2.succeeded) {
                  notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancelled_successfully'), 'success', 2000);
                  this.cancelConfirmationPopupVisible = false;
                  this.router.navigate(['/subscriptions']);
                } else {
                  notify(response2.message || this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
                }
              },
              error: (err2) => {
                console.error(`Endpoint ${endpoint} failed:`, err2);
                attemptCount++;
                tryNextEndpoint();
              }
            });
          };

          tryNextEndpoint();
        } else {
          // If DELETE fails, try POST approach
          console.log('DELETE failed, trying POST approach...');
          this.tryPostEndpoints();
        }
      }
    });
  }

  private tryPostEndpoints() {
    // Some APIs prefer POST for deletion with a specific action
    const postData: any = { id: this.id };

    const postEndpoints = [
      'Subscriptions/Delete',
      'Subscriptions/Cancel',
      'Subscription/Delete',
      'Subscription/Cancel'
    ];

    let attemptCount = 0;
    const tryNextPostEndpoint = () => {
      if (attemptCount >= postEndpoints.length) {
        this.isCancelling = false;
        console.error('All POST endpoint attempts failed');
        notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
        return;
      }

      const endpoint = postEndpoints[attemptCount];
      console.log(`Trying POST endpoint: ${endpoint}`);

      this.subscriptionsService.postAction(endpoint, postData).subscribe({
        next: (response: any) => {
          this.isCancelling = false;
          console.log('POST endpoint response:', response);
          if (response.succeeded) {
            notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancelled_successfully'), 'success', 2000);
            this.cancelConfirmationPopupVisible = false;
            this.router.navigate(['/subscriptions']);
          } else {
            notify(response.message || this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
          }
        },
        error: (err) => {
          console.error(`POST endpoint ${endpoint} failed:`, err);
          attemptCount++;
          tryNextPostEndpoint();
        }
      });
    };

    tryNextPostEndpoint();
  }



  navigateToAdminDetails(adminId: number) {
    // Navigate to admin details page or show admin details popup
    console.log('Navigate to admin details:', adminId);
    // You can implement navigation logic here based on your requirements
    // this.router.navigate(['/admin-details', adminId]);
  }

  goBack(): void {
    this.location.back();
  }

}
