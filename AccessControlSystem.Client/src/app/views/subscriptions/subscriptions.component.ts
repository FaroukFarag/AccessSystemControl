import { Component, OnInit } from '@angular/core';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';
import { SubscriptionService } from '../../services/subscriptions/subscription.service';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-subscriptions',
  imports: [
    DxButtonModule,
    DxDataGridModule,
    DxPopupModule
  ],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss'
})
export class SubscriptionsComponent implements OnInit {
  subscriptionsList: any;
  allowedPageSizes: (number | "auto")[] = [10, 20, 50];
  subscriptionTypes = [
    { id: 1, name: 'Standard' },
    { id: 2, name: 'Premium' },
    { id: 3, name: 'Enterprise' }
  ];
  subscriptionTypeEditorOptions: any;
  imageUploaderOptions: any;
  currentImageFile!: File;
  dateEditorOptions = {
    displayFormat: 'yyyy-MM-dd',
    type: 'date',
    pickerType: 'calendar',
    useMaskBehavior: true
  };

  constructor(private subscriptionService: SubscriptionService) {
  }

  ngOnInit() {
    this.getAllSubscriptions();

    this.loadSubscriptionTypeEditorOptions();

    this.configureImageUploader();
  }

  getAllSubscriptions() {
    this.subscriptionService.getAll('Subscriptions/GetAll').subscribe((data: any) => {
      this.subscriptionsList = data;
    })
  }

  loadSubscriptionTypeEditorOptions() {
    this.subscriptionTypeEditorOptions = {
      dataSource: this.subscriptionTypes,
      valueExpr: 'id',
      displayExpr: 'name',
    }
  }

  configureImageUploader() {
    this.imageUploaderOptions = {
      accept: 'image/*',
      uploadMode: 'useForm',
      multiple: false,
      labelText: 'Choose image',
      showFileList: false,
      onValueChanged: (e: any) => {
        this.currentImageFile = e.value[0];
      },
      isValid: (e: any) => {
        const isValid = e.value && e.value.length > 0;
        e.rule.isValid = isValid;
        return isValid;
      },
      validationMessage: 'Image is required'
    };
  }

  onRowInserting(e: any) {
    const formData = new FormData();
    
    formData.append('ImageFile', this.currentImageFile, this.currentImageFile.name);
    formData.append('CustomerName', e.data.customerName);
    formData.append('SubscriptionType', e.data.subscriptionType);
    formData.append('DeviceNumber', e.data.deviceNumber);
    formData.append('PaymentPerMonth', e.data.paymentPerMonth);
    formData.append('StartDate', formatDate(e.data.startDate, 'yyyy-MM-dd', 'en-US'));
    formData.append('EndDate', formatDate(e.data.endDate, 'yyyy-MM-dd', 'en-US'));
    formData.append('Note', e.data.note || '');
    
    this.subscriptionService.createWithImage('Subscriptions/Create', formData).subscribe(() => {
      this.getAllSubscriptions();
    });
  }

  onRowUpdating(e: any) {
    const formData = new FormData();
    const updatedData = { ...e.oldData, ...e.newData };
    
    formData.append('Id', updatedData.id);
    formData.append('ImageFile', this.currentImageFile, this.currentImageFile.name);
    formData.append('CustomerName', updatedData.customerName);
    formData.append('SubscriptionType', updatedData.subscriptionType);
    formData.append('DeviceNumber', updatedData.deviceNumber);
    formData.append('PaymentPerMonth', updatedData.paymentPerMonth);
    formData.append('StartDate', updatedData.startDate);
    formData.append('EndDate', updatedData.endDate);
    formData.append('Note', updatedData.note || '');

    this.subscriptionService.updateWithImage('Subscriptions/Update', updatedData).subscribe(
      () => {
        this.getAllSubscriptions();
      },
      (error) => {
        alert("Failed to update subscription: " + (error.error.message || "Unknown error"));
      }
    );
  }

  onRowRemoving(e: any) {
    const subscriptionId = e.data.id;
    this.subscriptionService.delete(`Subscriptions/Delete?id=${subscriptionId}`).subscribe(() => {
      this.getAllSubscriptions();
    });
  }

  onExporting(e: any) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Subscriptions');

    exportDataGrid({
      component: e.component,
      worksheet: worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Locations.xlsx');
      });
    });
  }
}
