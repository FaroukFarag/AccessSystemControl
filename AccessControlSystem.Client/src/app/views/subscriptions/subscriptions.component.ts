import { Component, OnInit } from '@angular/core';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';
import { SubscriptionService } from '../../services/subscriptions/subscription.service';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver';

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

  constructor(private subscriptionService: SubscriptionService) {
  }

  ngOnInit() {
    this.getAllSubscriptions();

    this.loadSubscriptionTypeEditorOptions();
  }

  loadSubscriptionTypeEditorOptions() {
    this.subscriptionTypeEditorOptions = {
      dataSource: this.subscriptionTypes,
      valueExpr: 'id',
      displayExpr: 'name',
    }
  }

  getAllSubscriptions() {
    this.subscriptionService.getAll('Subscriptions/GetAll').subscribe((data: any) => {
      this.subscriptionsList = data;
    })
  }

  onRowInserting(e: any) {
    this.subscriptionService.create('Subscriptions/Create',e.data).subscribe(() => {
      this.getAllSubscriptions(); 
    });
  }

  onRowUpdating(e: any) {
    const updatedData = { ...e.oldData, ...e.newData };
    this.subscriptionService.update('Subscriptions/Update', updatedData).subscribe(
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
