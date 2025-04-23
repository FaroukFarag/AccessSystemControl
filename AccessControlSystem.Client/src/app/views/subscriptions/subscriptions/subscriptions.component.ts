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
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';

import notify from 'devextreme/ui/notify';
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-subscriptions',
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
    DxFileUploaderModule,],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss'
})
export class SubscriptionsComponent {
  @ViewChild('subscriptionFormRef', { static: false }) dxForm: any;
  popupVisible: boolean = false;
  sortBy = ['Recent', 'date'];
  subscriptionssList: any;
  imageValidationError: string = '';
  subscriptionData = {
    subscriptionImageFile: null,
    subscriptionImageUrl: '',
    CustomerName: '',
    SubscriptionType: '',
    DeviceNumber: '',
    PaymentPerMonth: '',
    StartDate: '',
    EndDate: '',
    Note: ''
  };
  subscriptionTypeEditorOptions: any
  subscriptionTypes = [{ 'name': 'Type 1' },
  { 'name': 'Type 2' }]


  constructor(private router: Router, private subscriptionsService: SubscriptionService, private sanitizer: DomSanitizer) {
    this.subscriptionTypeEditorOptions = {
      dataSource: this.subscriptionTypes,
      valueExpr: 'name',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      value: 'Type 1',
      placeholder: 'Device type'
    };
  
}

  subscriptions: any = [
    {
      'id': '1',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '2',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "15"
    }, {
      'id': '3',
       'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '4',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "24"
    }, {
      'id': '5',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '6',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '7',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '8',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '9',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '10',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '11',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '12',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '13',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '14',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '15',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '16',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '17',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '18',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '19',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '20',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '21',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '22',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '23',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '24',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '25',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '26',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, 
  ]



  ngOnInit() {
    this.getAllSubscriptions();
  }

  getAllSubscriptions() {
    this.subscriptionsService.getAll('Subscriptions/GetAll').subscribe((data: any) => {
      this.subscriptionssList = data;
      console.log("subscriptionssList", this.subscriptionssList);

    })
  }

  showAddDevicePopup() {
    this.subscriptionData = {
      subscriptionImageFile: null,
      subscriptionImageUrl: '',
      CustomerName: '',
      SubscriptionType: '',
      DeviceNumber: '',
      PaymentPerMonth: '',
      StartDate: '',
      EndDate: '',
      Note: ''
    };
    this.imageValidationError = '';
    this.popupVisible = true;
  }
  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
  navigateToDetailsPage() {
    this.router.navigate(['/subscription-details']);
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
      
    }
    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

  
    const formData = new FormData();

    formData.append('ImageFile', this.subscriptionData.subscriptionImageFile || '');
    formData.append('ImageEncode', this.subscriptionData.subscriptionImageUrl || '');
    formData.append('CustomerName', this.subscriptionData.CustomerName);
    formData.append('SubscriptionType', this.subscriptionData.SubscriptionType); // ensure this is numeric
    formData.append('DeviceNumber', this.subscriptionData.DeviceNumber);
    formData.append('PaymentPerMonth', '100'); // Replace with actual field or input
    formData.append('StartDate', new Date().toISOString());
    formData.append('EndDate', new Date().toISOString());
    formData.append('Note', 'Added via UI');

    this.subscriptionsService.create('Subscriptions/Create', formData as any).subscribe({
      next: (response) => {
        notify('Device created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllSubscriptions();
      },
      error: (err) => {
        notify('Error creating device', 'error', 2000);
        console.error(err);
      }
    });
  }


}
