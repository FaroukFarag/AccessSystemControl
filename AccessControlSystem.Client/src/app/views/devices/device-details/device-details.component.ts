import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';
import { DeviceService } from '../../../services/devices/device.service';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [DxDataGridModule, TranslatePipe],
  templateUrl: './device-details.component.html',
  styleUrl: './device-details.component.scss'
})
export class DeviceDetailsComponent implements OnInit {
  direction: 'ltr' | 'rtl' = 'ltr';
  dataSource: any[] = [];
  deviceDetails: any = null;
  deviceId: string = '';

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private languageService: LanguageService
  ) { }


  ngOnInit() {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.route.queryParams.subscribe(params => {
      this.deviceId = params['id'];
      if (this.deviceId) {
        this.getDeviceDetails(this.deviceId);
      }
    });

    this.getDevicesTraffic();
  }


  getDeviceDetails(id: string) {
    this.deviceService.getById('Devices/Get', id).subscribe({
      next: (data: any) => {
        this.deviceDetails = data.resultData;
        console.log('Device Details:', this.deviceDetails);
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }

  getDevicesTraffic() {
    this.deviceService.getAll('Devices/GetDevicesTraffic').subscribe((data: any) => {
      this.dataSource = data.resultData;
    })
  }
}
