import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Router } from '@angular/router';
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
import { DxDropDownButtonModule, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-subscriptions',
  standalone: true,
  imports: [
    CommonModule,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxDropDownButtonModule,
    DxFileUploaderModule
  ],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss'
})
export class SubscriptionsComponent {
  @ViewChild('subscriptionFormRef', { static: false }) dxForm: any;
  popupVisible: boolean = false;
  sortBy = ['Recent', 'date'];
  subscriptions: any;
  imageValidationError: string = '';
  subscriptionData = {
    subscriptionImageFile: null,
    subscriptionImageUrl: '',
    customerName: '',
    subscriptionType: '',
    adminNumber: '',
    deviceNumber: '',
    cardNumber: '',
    paymentPerMonth: '',
    startDate: new Date(),
    endDate: new Date(),
    note: ''
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

  constructor(
    private router: Router,
    private subscriptionsService: SubscriptionService,
    private sanitizer: DomSanitizer) {
    this.subscriptionTypeEditorOptions = {
      valueExpr: 'id',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      placeholder: 'Subscription type'
    };
  }

  ngOnInit() {
    this.getAllSubscriptions();
  }

  getAllSubscriptions() {
    this.subscriptionsService.getAll('Subscriptions/GetAll').subscribe((data: any) => {
      this.subscriptions = data;
      console.log("subscriptionssList", this.subscriptions);

    })
  }

  showAddDevicePopup() {
    this.subscriptionData = {
      subscriptionImageFile: null,
      subscriptionImageUrl: '',
      customerName: '',
      subscriptionType: '',
      adminNumber: '',
      deviceNumber: '',
      cardNumber: '',
      paymentPerMonth: '',
      startDate: new Date(),
      endDate: new Date(),
      note: ''
    };
    this.imageValidationError = '';
    this.popupVisible = true;
  }

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }

  navigateToDetailsPage(id: number) {
    this.router.navigate(['/subscription-details', id]);
  }

  sanitizeImage(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChange(e: any) {
    const file = e.value[0];
    if (file) {
      this.subscriptionData.subscriptionImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.subscriptionData.subscriptionImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  submitSubscription() {
    if (!this.subscriptionData.subscriptionImageFile) {
      this.imageValidationError = 'Image is required';
      return;
    }

    const result = this.dxForm.instance.validate();

    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const start = new Date(this.subscriptionData.startDate);
    const end = new Date(this.subscriptionData.endDate);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      notify('Invalid start or end date', 'error', 2000);
      return;
    }

    const startFormatted = start.toISOString().split('T')[0];
    const endFormatted = end.toISOString().split('T')[0];

    console.log('Start Date:', startFormatted);
    console.log('End Date:', endFormatted);


    const selectedType = this.subscriptionTypes.find(t => t.id === Number(this.subscriptionData.subscriptionType));
    const subscriptionTypeName = selectedType ? selectedType.name : '';

    const formData = new FormData();

    formData.append('CustomerName', this.subscriptionData.customerName);
    formData.append('SubscriptionType', Number(this.subscriptionData.subscriptionType).toString());
    formData.append('AdminNumber', Number(this.subscriptionData.adminNumber).toString());
    formData.append('DeviceNumber', Number(this.subscriptionData.deviceNumber).toString());
    formData.append('CardNumber', Number(this.subscriptionData.cardNumber).toString());
    formData.append('PaymentPerMonth', Number(this.subscriptionData.paymentPerMonth).toString());
    formData.append('StartDate', startFormatted);
    formData.append('EndDate', endFormatted);
    formData.append('Note', this.subscriptionData.note || '');
    formData.append('ImagePath', '');
    formData.append('ImageEncode', this.subscriptionData.subscriptionImageUrl || '');
    formData.append('ImageFile', this.subscriptionData.subscriptionImageFile || '');

    this.subscriptionsService.create('Subscriptions/Create', formData as any).subscribe({
      next: (response) => {
        notify('Device created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllSubscriptions();
      },
      error: (err) => {
        console.error('Error creating device', err);

        if (err && err.error && err.error.errors) {
          console.error('Validation Errors:', err.error.errors);
        }

        notify('Error creating device', 'error', 2000);
      }
    });
  }
}
