import { Component, OnInit } from '@angular/core';
import { DxButtonModule, DxDataGridModule, DxPopupModule } from 'devextreme-angular';
import { UnitService } from '../../services/units/unit.service';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-units',
  imports: [
    DxButtonModule,
    DxDataGridModule,
    DxPopupModule
  ],
  templateUrl: './units.component.html',
  styleUrl: './units.component.scss'
})
export class UnitsComponent implements OnInit {
  unitsList: any;
  allowedPageSizes: (number | "auto")[] = [10, 20, 50];

  constructor(private unitService: UnitService) {
  }

  ngOnInit() {
    this.getAllUnits();
  }

  getAllUnits() {
    this.unitService.getAll('Units/GetAll').subscribe((data: any) => {
      this.unitsList = data;
    })
  }

  onRowInserting(e: any) {
    this.unitService.create('Units/Create',e.data).subscribe(() => {
      this.getAllUnits(); 
    });
  }

  onRowUpdating(e: any) {
    const updatedData = { ...e.oldData, ...e.newData };
    this.unitService.update('Units/Update', updatedData).subscribe(
      () => {
        this.getAllUnits(); 
      },
      (error) => {
        alert("Failed to update unit: " + (error.error.message || "Unknown error"));
      }
    );
  }

  onRowRemoving(e: any) {
    const unitId = e.data.id;
    this.unitService.delete(`Units/Delete?id=${unitId}`).subscribe(() => {
      this.getAllUnits(); 
    });
  }

  onExporting(e: any) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Units');

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
