import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
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
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';
import notify from 'devextreme/ui/notify';
import { DxFormComponent } from 'devextreme-angular';
import { DeviceService } from '../../../services/devices/device.service';

@Component({
  selector: 'app-subscription-details',
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
    DxFileUploaderModule,],
  templateUrl: './subscription-details.component.html',
  styleUrl: './subscription-details.component.scss'
})
export class SubscriptionDetailsComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  popupVisible: boolean = false;
  // subscription = {
  //   subscriptionTypeName: 'Standard',
  //   paymentPerMonth: 240,
  //   startDate: '31-03-2024',
  //   endDate: '31-03-2026',
  //   usedAdmins: 3,
  //   adminNumber: 5,
  //   usedDevices: 12,
  //   deviceNumber: 20,
  //   usedCards: 2,
  //   cardNumber: 3,
  //   devices: Array.from({ length: 12 }).map((_, i) => ({
  //     name: `Device ${i + 1}`,
  //     active: 'Active',
  //     macAddress: `50:80:D0:63:XX:${(i + 10).toString(16).toUpperCase()}`,
  //     deviceTypeName: 'Type name',
  //     image: '/assets/images/device.png'
  //   }))
  // };
  subscription: any;
  imageValidationError: string = '';
  deviceListEditorOptions: any

  deviceData = {
    selectedDevices: [] as number[],  
  };
  devicesList: any;
  constructor(
    private route: ActivatedRoute,
    private subscriptionsService: SubscriptionService,
    private deviceService: DeviceService,) {

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
 
  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;

    this.subscriptionsService.getById('Subscriptions/Get', id).subscribe(data => {
      this.subscription = data;
    })
  }

  getProgress(used: number, total: number): number {
    return (used / total) * 100;
  }

  getAllDevices() {
    this.deviceService.getAll('Devices/GetAll').subscribe((data: any) => {
      this.devicesList = data;
      console.log("DEVICCES", this.devicesList);

    })
  }


  showAddDevicePopup() {
    this.popupVisible = true;
    this.getAllDevices();
    this.deviceData = {
      selectedDevices: [] as number[],  

    };
  }




  submitDevice() {
    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const subscriptionId = +this.route.snapshot.paramMap.get('id')!;

    const selectedDevices = this.deviceData.selectedDevices;
    if (!selectedDevices || selectedDevices.length === 0) {
      notify('Please select at least one device.', 'warning', 1500);
      return;
    }
debugger
    const sd = selectedDevices.map(deviceId => {
      return { subscriptionId, deviceId }
    });

    this.deviceService.create('SubscriptionsDevices/CreateRange', sd as any).subscribe({
      next: () => {
        notify(`Devices linked successfully`, 'success', 1500);
      },
      error: (err) => {
        notify('Error linking devices', 'error', 2000);
        console.error(err);
      }
    });

    this.popupVisible = false;
  }

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
}
