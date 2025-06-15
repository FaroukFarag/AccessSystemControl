import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UnitService } from '../../../services/units/unit.service';
import notify from 'devextreme/ui/notify';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxFormModule,
} from 'devextreme-angular';

@Component({
  selector: 'app-unit-details',
  standalone: true,
  imports: [DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxFormModule,],
  templateUrl: './unit-details.component.html',
  styleUrl: './unit-details.component.scss'
})
export class UnitDetailsComponent {
  unitId: any;
  unitDetails: any = null;
  assignToOwner_popupVisible = false;
  constructor(private route: ActivatedRoute, private unitsService: UnitService) { }
  ngOnInit() {



    this.route.queryParams.subscribe(params => {
      this.unitId = params['id'];
      console.log('Unit ID:', this.unitId);
      if (this.unitId) {
        this.getUnitDetails(this.unitId);
      }
    });

  }



  getUnitDetails(id: string) {
    this.unitsService.getById('Units/Get', id).subscribe({
      next: (data: any) => {
        this.unitDetails = data;
        console.log('Unit Details:', this.unitDetails);
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }
  openAssignToOwnerPopup() {
    this.assignToOwner_popupVisible = true;
  }

}
