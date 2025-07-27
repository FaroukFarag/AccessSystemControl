import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import {
  DxDataGridModule,
  DxButtonModule,
  DxFormModule,
  DxTemplateModule,
  DxPopupModule,
  DxSelectBoxModule,
} from 'devextreme-angular';
import { AccessGroupService } from '../../services/access-groups/access-group.service';
import { DxFormComponent } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { DeviceService } from '../../services/devices/device.service';
import { AccessGroup } from '../../models/access-group/access-group';

@Component({
  selector: 'app-access-groups',
  standalone: true,
  imports: [
    DxDataGridModule,
    DxButtonModule,
    DxFormModule,
    DxTemplateModule,
    DxPopupModule,
    DxSelectBoxModule
  ],
  templateUrl: './access-groups.component.html',
  styleUrl: './access-groups.component.scss'
})
export class AccessGroupsComponent {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  accessGroupsList: any;
  sites = [];
  schedules = [];
  devicesList: any;
  deviceListEditorOptions: any
  deviceData = {
    name: undefined,
    siteId: undefined,
    selectedDevices: [] as number[],
  };

  constructor(private router: Router, private accessGroupService: AccessGroupService, private deviceService: DeviceService) {
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
    this.getAllSites();
    this.getAllSchedules();
    this.getAllDevices();
    this.getAllAccessGroups();
  }

  getAllAccessGroups() {
    this.accessGroupService.getAll('AccessGroups/GetAll').subscribe((data: any) => {
      this.accessGroupsList = data.resultData;
      console.log("accessGroupsList", this.accessGroupsList);

    })
  }

  getAllSites() {
    this.accessGroupService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;

      else
        notify('Error getting sites', 'error', 2000);
    })
  }

  getAllSchedules() {
    this.accessGroupService.getAll('AirfobSchedules/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.schedules = data.resultData.schedules;

      else
        notify('Error getting schedules', 'error', 2000);
    })
  }

  getAllDevices() {
    this.deviceService.getAll(`Devices/GetAll`).subscribe((data: any) => {
      this.devicesList = data.resultData;

    })
  }

  submit(e: any) {
    const newData = e.data;

    const payload: AccessGroup = {
      name: newData.name,
      siteId: newData.siteId,
      scheduleId: newData.scheduleId,
      devices: newData.devices
    } as AccessGroup;

    e.cancel = true;

    this.accessGroupService.create('AccessGroups/Create', payload).subscribe({
      next: () => {
        notify('Access group created successfully', 'success', 1500);

        e.component.cancelEditData();

        this.getAllAccessGroups();
      },
      error: (err) => {
        notify('Failed to create access group', 'error', 2000);
        console.error(err);
      }
    });
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
