import { Component } from '@angular/core';
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
} from 'devextreme-angular';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import { DeviceService } from '../../../services/devices/device.service';

import notify from 'devextreme/ui/notify';
@Component({
  selector: 'app-devices',
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
    ],
  templateUrl: './devices.component.html',
  styleUrl: './devices.component.scss',
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DevicesComponent {

  popupVisible: boolean = false;
  sortBy = ['Recent', 'date'];
  devicesList: any;
  constructor(private router: Router, private deviceService: DeviceService) { }

  devices: any = [
    {
    'id':'1',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'2',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'3',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'4',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'5',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'6',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'7',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'8',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'9',
    'name': 'Device Name',
    'status': 'Active',
      'macAddress': " S00-B0-98",
      'deviceType': 'Type name'
  },  {
    'id':'10',
    'name': 'Device Name',
    'status': 'Active',
    'macAddress': " S00-B0-98",
    'deviceType':'Type name'
  }, {
    'id':'11',
    'name': 'Device Name',
    'status': 'Active',
    'macAddress': " S00-B0-98",
    'deviceType':'Type name'
  }, {
    'id':'12',
    'name': 'Device Name',
    'status': 'Active',
    'macAddress': " S00-B0-98",
    'deviceType':'Type name'
  }, {
    'id':'13',
    'name': 'Device Name',
    'status': 'Active',
    'macAddress': " S00-B0-98",
    'deviceType':'Type name'
  }, {
    'id':'14',
    'name': 'Device Name',
    'status': 'Active',
    'macAddress': " S00-B0-98",
    'deviceType':'Type name'
  },
  ]

  showAddDevicePopup() {
    this.popupVisible = true;
  }
  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
  navigateToDetailsPage() {
    this.router.navigate(['/device-details']);
  }



  ngOnInit() {
    this.getAllDevices();
    console.log("DEVICCES", this.devicesList);
  }



  getAllDevices() {
    this.deviceService.getAll('Devices/GetAll').subscribe((data: any) => {
      this.devicesList = data;
    })
  }

}
