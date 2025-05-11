import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { SubscriptionService } from '../../../services/subscriptions/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
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
  id!: number;
  popupVisible: boolean = false;
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
    private deviceService: DeviceService,
    private router: Router,) {

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
    this.id = +this.route.snapshot.paramMap.get('id')!;

    this.subscriptionsService.getById('Subscriptions/Get', this.id).subscribe(data => {
      this.subscription = data;
      this.calculateTotalPayment();

    });
  }

 



  totalPayment: number = 0;

  calculateTotalPayment() {
    const payment = Number(this.subscription?.paymentPerMonth);
    const months = Number(this.subscription?.monthNumber);
    if (!isNaN(payment) && !isNaN(months)) {
      this.totalPayment = payment * months;
    } else {
      this.totalPayment = 0;
    }
  }

  getProgress(used: number, total: number): number {
    return (used / total) * 100;
  }

  getAllDevices() {
    this.deviceService.getAll(`Devices/GetAvailableDevicesForSubscription?subscriptionId=${this.id}`).subscribe((data: any) => {
      this.devicesList = data;

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

    const selectedDevices = this.deviceData.selectedDevices;
    if (!selectedDevices || selectedDevices.length === 0) {
      notify('Please select at least one device.', 'warning', 1500);
      return;
    }

    const subscriptionsDevices = selectedDevices.map(deviceId => {
      return { subscriptionId: this.id, deviceId }
    });

    this.deviceService.create('SubscriptionsDevices/CreateRange', subscriptionsDevices as any).subscribe({
      next: () => {
        this.getAllDevices();
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
  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }
}
