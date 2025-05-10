import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import {
  DxDataGridModule,
  DxButtonModule,
  DxFormModule,
  DxTemplateModule,
  DxPopupModule,
 } from 'devextreme-angular';
import { AccessGroupservice } from '../../services/access-groups/access-groups.service';
import { DxFormComponent } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { DeviceService } from '../../services/devices/device.service';

@Component({
  selector: 'app-access-groups',
  standalone: true,
  imports: [
    DxDataGridModule,
    DxButtonModule,
    DxFormModule,
    DxTemplateModule,
    DxPopupModule,],
  templateUrl: './access-groups.component.html',
  styleUrl: './access-groups.component.scss'
})
export class AccessGroupsComponent {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  accessGroupsList: any;
  devicesList: any;
  deviceListEditorOptions: any
  deviceData = {
    selectedDevices: [] as number[],
  };
  constructor(private router: Router, private accessGroupsService: AccessGroupservice, private deviceService: DeviceService) {
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

  ngOnInit() {
    this.getAllAccessGroups();
  }
  getAllAccessGroups() {
    this.accessGroupsService.getAll('AccessGroups/GetAll').subscribe((data: any) => {
      this.accessGroupsList = data;
      console.log("accessGroupsList", this.accessGroupsList);

    })
  }
  getAllDevices() {
    this.deviceService.getAll(`Devices/GetAll`).subscribe((data: any) => {
      this.devicesList = data;

    })
  }

  submitDevice() {
    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }
  }


  goToAccessGroupDevices(accessGroup: any) {
    console.log('Navigating to group:', accessGroup);
    const accessGroupId = accessGroup?.id;

    if (!accessGroupId) {
      console.error('Missing access group ID!');
      return;
    }

    this.router.navigate(['/access-groups-devices', accessGroupId]);
  }


}
