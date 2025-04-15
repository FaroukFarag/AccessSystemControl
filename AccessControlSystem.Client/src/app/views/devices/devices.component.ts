import { Component, OnInit } from '@angular/core';
import { DeviceService } from '../../services/devices/device.service';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';

@Component({
  selector: 'app-devices',
  imports: [
    DxButtonModule,
    DxDataGridModule,
    DxPopupModule
  ],
  templateUrl: './devices.component.html',
  styleUrl: './devices.component.scss'
})
export class DevicesComponent implements OnInit {
  devicesList: any;
  allowedPageSizes: (number | "auto")[] = [10, 20, 50];
  deviceTypes = [
    { id: 1, name: 'Airfob Edge Reader' },
    { id: 2, name: 'Airfob Edge Reader Ultimate' },
    { id: 3, name: 'Airfob Tag' },
    { id: 4, name: 'Airfob Patch' },
    { id: 5, name: 'Suprema X-Station 2' },
    { id: 6, name: 'Wireless Door Locks' },
  ];
  deviceTypeEditorOptions: any;

  constructor(private deviceService: DeviceService) {
  }

  ngOnInit() {
    this.getAllDevices();

    this.loadDeviceTypeEditorOptions();
  }

  loadDeviceTypeEditorOptions() {
    this.deviceTypeEditorOptions = {
      dataSource: this.deviceTypes,
      valueExpr: 'id',
      displayExpr: 'name',
    }
  }

  getAllDevices() {
    this.deviceService.getAll('Devices/GetAll').subscribe((data: any) => {
      this.devicesList = data;
    })
  }

  onRowInserting(e: any) {
    this.deviceService.create('Devices/Create',e.data).subscribe(() => {
      this.getAllDevices(); 
    });
  }

  onRowUpdating(e: any) {
    const updatedData = { ...e.oldData, ...e.newData };
    this.deviceService.update('Devices/Update', updatedData).subscribe(
      () => {
        this.getAllDevices(); 
      },
      (error) => {
        alert("Failed to update device: " + (error.error.message || "Unknown error"));
      }
    );
  }

  onRowRemoving(e: any) {
    const deviceId = e.data.id;
    this.deviceService.delete(`Devices/Delete?id=${deviceId}`).subscribe(() => {
      this.getAllDevices(); 
    });
  }

  onExporting(e: any) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Devices');

    exportDataGrid({
      component: e.component,
      worksheet: worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Devices.xlsx');
      });
    });
  }
}
