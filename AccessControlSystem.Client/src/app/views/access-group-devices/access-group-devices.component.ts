import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 
import notify from 'devextreme/ui/notify';
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
import { DxFormComponent } from 'devextreme-angular';
import { CommonModule } from '@angular/common'; 
import { DeviceService } from '../../services/devices/device.service';
@Component({
  selector: 'app-access-group-devices',
  standalone: true,
  imports: [    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    CommonModule,
    DxFormModule,
  ],
  templateUrl: './access-group-devices.component.html',
  styleUrls: ['./access-group-devices.component.scss']
})
export class AccessGroupDevicesComponent implements OnInit {
  devices: any[] = [];
  availableDevicesList: any[] = [];
  accessGroupId!: string;
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  selectedDeviceIds: string[] = [];
  popupVisible: boolean = false;
  groupDevice_popupVisible: boolean = false;
  sortBy = ['Recent', 'date'];
  devicesList: any;
  imageValidationError: string = '';
  deviceData = {
    deviceImageFile: null,
    deviceImageUrl: '',
    deviceName: '',
    deviceType: '',
    macAddress: ''
  };
  groupDeviceseData = {


  };
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private deviceService: DeviceService
  ) { }

  ngOnInit() {
    this.accessGroupId = this.route.snapshot.paramMap.get('id')!;
  }

  submitSelectedDevices() {
    if (this.selectedDeviceIds.length === 0) {
      notify('No devices selected', 'warning', 1500);
      return;
    }
    const payload = this.selectedDeviceIds.map(deviceId => ({
      accessGroupId: this.accessGroupId,
      deviceId: deviceId
    }));
    this.deviceService.create('http://localhost:5273/api/AccessGroupsDevices/CreateRange', payload as any)
      .subscribe({
        next: () => {
          notify(`${this.selectedDeviceIds.length} devices assigned successfully`, 'success', 2000);
          this.groupDevice_popupVisible = false;
          this.selectedDeviceIds = [];
        },
        error: (error) => {
          console.error('API error:', error);
          notify('Error assigning devices', 'error', 2000);
        }
      });

  }


  openGroupDevicesPopup() {
    this.groupDevice_popupVisible = true;
    this.getAllDevices();
    this.selectedDeviceIds = [];
  }

  getAllDevices() {
    this.deviceService.getAll(`Devices/GetAvailableDevicesForAccessGroup?accessGroupId=${this.accessGroupId}`).subscribe((data: any) => {
      this.devicesList = data;
    })
  }

  toggleDeviceSelection(deviceId: string) {
    const index = this.selectedDeviceIds.indexOf(deviceId);
    if (index > -1) {
      this.selectedDeviceIds.splice(index, 1);
    } else {
      this.selectedDeviceIds.push(deviceId);
    }
  }

  get popupHeight(): number {
    return this.devicesList?.length === 0 ? 220 : 450;
  }
  get popupWidth(): number {
    return this.devicesList?.length === 0 ? 500 : 950;
  }

}
