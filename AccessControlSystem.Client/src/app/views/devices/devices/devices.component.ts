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
import { DeviceService } from '../../../services/devices/device.service';
import { DomSanitizer } from '@angular/platform-browser';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';


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
    DxFileUploaderModule,
    ],
  templateUrl: './devices.component.html',
  styleUrl: './devices.component.scss',
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DevicesComponent {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  popupVisible: boolean = false;
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
  deviceTypeEditorOptions: any
 
  deviceTypes = [
    {
      'id': '1',
      'name': 'Airfob Edge Reader'
    },
    {
      'id': '2',
      'name': 'Airfob Edge Reader Ultimate'
    },
    {
      'id': '3',
      'name': 'Airfob Tag'
    },
    {
      'id': '4',
      'name': 'Airfob Patch'
    },  {
      'id': '5',
      'name': 'Suprema X-Station 2'
    },  {
      'id': '6',
      'name': 'Wireless Door Locks'
    },
  ]
  constructor(private router: Router, private deviceService: DeviceService, private sanitizer: DomSanitizer) {

    this.deviceTypeEditorOptions = {
      dataSource: this.deviceTypes,
      valueExpr: 'name',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      value: 'Type 1',
      placeholder: 'Device type'
    };
  }

  ngOnInit() {
    this.getAllDevices();
  }

  getAllDevices() {
    this.deviceService.getAll('Devices/GetAll').subscribe((data: any) => {
      this.devicesList = data;
      console.log("DEVICCES", this.devicesList);

    })
  }

  showAddDevicePopup() {
    this.popupVisible = true;
    this.deviceData = {
      deviceImageFile: null,
      deviceImageUrl: '',
      deviceName: '',
      deviceType: '',
      macAddress: ''
    };
  }



  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }


  sanitizeImage(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChange(e: any) {
    const file = e.value[0];
    if (file) {
      this.deviceData.deviceImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.deviceData.deviceImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  submitDevice() {
    this.imageValidationError = '';
    if (!this.deviceData.deviceImageFile) {
      this.imageValidationError = 'Image is required';
    }

    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const formData = new FormData();
    formData.append('ImageFile', this.deviceData.deviceImageFile || '');
    formData.append('ImageEncode', this.deviceData.deviceImageUrl || '');
    formData.append('Name', this.deviceData.deviceName);
    formData.append('DeviceTypeName', this.deviceData.deviceType);
    formData.append('DeviceType', '1'); 
    formData.append('MacAddress', this.deviceData.macAddress);
    formData.append('Active', 'true');

    this.deviceService.create('Devices/Create', formData as any).subscribe({
      next: (response) => {
        notify('Device created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllDevices(); 
      },
      error: (err) => {
        notify('Error creating device', 'error', 2000);
        console.error(err);
      }
    });
  }

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }

}
