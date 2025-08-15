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
@Component({
  selector: 'app-admin-dashboard',
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
    DxFileUploaderModule,],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit, OnDestroy {
  @ViewChild('subscriptionFormRef', { static: false }) dxForm: any;

  isSidebarOpen = true;
  private sidebarSubscription?: Subscription;
  devicesCount: any;
  subscriptionsCount: any;
  subscriptions: any;
  imageValidationError: string = '';
  popupVisible: boolean = false;
  subscriptionsLAstMonthCount!: number;
  devicesLastMonthCount!: number;
  subscriptionData = {
    SubscriptionImageFile: null,
    SubscriptionImageUrl: '',
    CustomerName: '',
    SubscriptionType: '',
    SubscriptionTypeName: '',
    AdminNumber: '',
    DeviceNumber: '',
    CardNumber: '',
    MonthNumber: '',
    UsedAdmins: 0,
    UsedDevices: 0,
    UsedCards: 0,
    PaymentPerMonth: '',
    StartDate: new Date(),
    EndDate: new Date(),
    Note: '',
    RenewalInfo: '',
    Devices: [],
    ImagePath: '',
    Id: 0
  };

  subscriptionTypeEditorOptions: any
  subscriptionTypes = [
    {
      id: 1,
      name: 'Standard'
    },
    {
      id: 2,
      name: 'Premium'
    },
    {
      id: 3,
      name: 'Enterprise'
    }
  ]
  devices = [
    {
      name: 'Smart TV Living Room',
      startDate: '2024-01-15',
      endDate: '2024-12-15',
      timeRemaining: '8 months'
    },
    {
      name: 'Mobile Device - iPhone',
      startDate: '2024-02-01',
      endDate: '2024-11-01',
      timeRemaining: '7 months'
    },
    {
      name: 'Laptop - MacBook Pro',
      startDate: '2024-03-10',
      endDate: '2025-03-10',
      timeRemaining: '11 months'
    },
    {
      name: 'Tablet - iPad Air',
      startDate: '2024-01-20',
      endDate: '2024-10-20',
      timeRemaining: '6 months'
    }
  ];


  constructor(
    private sidebarService: SidebarService,
    public languageService: LanguageService,
    private router: Router,
    private subscriptionsService: SubscriptionService,
    private userService: UserService,
    private sanitizer: DomSanitizer ) {
    this.subscriptionTypeEditorOptions = {
      valueExpr: 'id',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      placeholder: 'Subscription type'
    };
  }

  ngOnInit() {
    this.sidebarSubscription = this.sidebarService.isOpen$.subscribe(
      (isOpen: boolean) => this.isSidebarOpen = isOpen
    );
   
    this.getSubscriptionsCount();
    this.getDevicesCount();
    this.getAllSubscriptions();
    this.getSubscriptionsLastMonthCount();
    this.getDevicesLAstMonthCount();
  }

  ngOnDestroy() {
    if (this.sidebarSubscription) {
      this.sidebarSubscription.unsubscribe();
    }
  }

  get sidebarOpen() {
    return this.isSidebarOpen;
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


  getSubscriptionsCount() {
    this.userService.getAll('Subscriptions/GetSubscriptionsCount').subscribe((data: any) => {
      this.subscriptionsCount = data.resultData;
    })
  }
  getSubscriptionsLastMonthCount() {
    this.userService.getAll('Subscriptions/GetLastMonthSubscriptionsCount').subscribe((data: any) => {
      this.subscriptionsLAstMonthCount = data.resultData;
    })
  }
  getDevicesCount() {
    this.userService.getAll('Devices/GetDevicesCount').subscribe((data: any) => {
      this.devicesCount = data.resultData;
    })
  }

   getDevicesLAstMonthCount() {
     this.userService.getAll('Devices/GetLastMonthDevicesCount').subscribe((data: any) => {
      this.devicesLastMonthCount = data.resultData;
    })
  }


  navigateToSubscriptions() {
    this.router.navigate(['/subscriptions']);
  }

  navigateToDetailsPage(id: number) {
    this.router.navigate(['/subscription-details', id]);
  }



  showAddSubscriptionPopup() {
    this.subscriptionData = {
      SubscriptionImageFile: null,
      SubscriptionImageUrl: '',
      CustomerName: '',
      SubscriptionType: '',
      SubscriptionTypeName: '',
      AdminNumber: '',
      DeviceNumber: '',
      CardNumber: '',
      MonthNumber: '',
      UsedAdmins: 0,
      UsedDevices: 0,
      UsedCards: 0,
      PaymentPerMonth: '',
      StartDate: new Date(),
      EndDate: new Date(),
      Note: '',
      RenewalInfo: '',
      Devices: [],
      ImagePath: '',
      Id: 0
    };


    this.imageValidationError = '';
    this.popupVisible = true;
  }


  /*Add New Subscription Functionality */

  sanitizeImage(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChange(e: any) {
    const file = e.value[0];
    if (file) {
      this.subscriptionData.SubscriptionImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.subscriptionData.SubscriptionImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  submitSubscription() {
    if (!this.subscriptionData.SubscriptionImageFile) {
      this.imageValidationError = this.languageService.translate('validation.image_required');
      return;
    }

    const result = this.dxForm.instance.validate();

    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    const start = new Date(this.subscriptionData.StartDate);
    const end = new Date(this.subscriptionData.EndDate);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      notify(this.languageService.translate('messages.error.invalid_dates'), 'error', 2000);
      return;
    }

    const startFormatted = start.toISOString().split('T')[0];
    const endFormatted = end.toISOString().split('T')[0];

    console.log('Start Date:', startFormatted);
    console.log('End Date:', endFormatted);


    const selectedType = this.subscriptionTypes.find(t => t.id === Number(this.subscriptionData.SubscriptionType));
    this.subscriptionData.SubscriptionTypeName = selectedType?.name || '';
    debugger
    const formData = new FormData();

    formData.append('CustomerName', this.subscriptionData.CustomerName);
    formData.append('SubscriptionType', Number(this.subscriptionData.SubscriptionType).toString());
    formData.append('SubscriptionTypeName', this.subscriptionData.SubscriptionTypeName);
    formData.append('AdminNumber', Number(this.subscriptionData.AdminNumber).toString());
    formData.append('DeviceNumber', Number(this.subscriptionData.DeviceNumber).toString());
    formData.append('CardNumber', Number(this.subscriptionData.CardNumber).toString());
    formData.append('MonthNumber', Number(this.subscriptionData.MonthNumber || 0).toString());
    formData.append('UsedAdmins', this.subscriptionData.UsedAdmins.toString());
    formData.append('UsedDevices', this.subscriptionData.UsedDevices.toString());
    formData.append('UsedCards', this.subscriptionData.UsedCards.toString());
    formData.append('PaymentPerMonth', Number(this.subscriptionData.PaymentPerMonth).toString());
    formData.append('StartDate', startFormatted);
    formData.append('EndDate', endFormatted);
    formData.append('Note', this.subscriptionData.Note || '');
    formData.append('RenewalInfo', this.subscriptionData.RenewalInfo || '');
    formData.append('Devices', JSON.stringify(this.subscriptionData.Devices || []));
    formData.append('ImagePath', '');
    formData.append('ImageFile', this.subscriptionData.SubscriptionImageFile);
    formData.append('Id', this.subscriptionData.Id.toString());


    this.subscriptionsService.create('Subscriptions/Create', formData as any).subscribe({
      next: (response) => {
        notify(this.languageService.translate('messages.success.subscription_created'), 'success', 1500);
        this.popupVisible = false;
        this.getAllSubscriptions();
      },
      error: (err) => {
        if (err && err.error && err.error.errors) {
        }

        notify(this.languageService.translate('messages.error.subscription_creation'), 'error', 2000);
      }
    });
  }
  validateStartDate = (e: any): boolean => {
    console.log("E", e.value);
    if (!e.value) return false;

    const selectedDate = new Date(e.value);
    const today = new Date();
    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    return selectedDate >= today;
  }
}
