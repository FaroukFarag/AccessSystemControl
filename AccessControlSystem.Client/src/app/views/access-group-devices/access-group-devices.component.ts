import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
import { DxDataGridModule, DxDataGridTypes } from 'devextreme-angular/ui/data-grid';

import { DxFormComponent } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/devices/device.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { LanguageService } from '../../services/language/language.service';
@Component({
  selector: 'app-access-group-devices',
  standalone: true,
  imports: [DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    CommonModule,
    DxFormModule,
    DxDataGridModule,
    TranslatePipe
  ],
  templateUrl: './access-group-devices.component.html',
  styleUrls: ['./access-group-devices.component.scss']
})
export class AccessGroupDevicesComponent {
  groupId!: number;
  accessGroup: any = null;
  direction: 'ltr' | 'rtl' = 'ltr';
  
  // Column captions for the data grid
  columnCaptions = {
    trafficType: '',
    time: '',
    date: '',
    deviceMacAddress: '',
    image: ''
  };
  dataSource: any[] = [

    {

      "Traffic type": "Check In",

      "Time": "12:00 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5E",

      "image": "path/to/image1.png"

    },

    {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    }, {

      "Traffic type": "Check Out",

      "Time": "12:30 PM",

      "Date": new Date(2023, 9, 1),

      "DeviceMacAddress": "00:1A:2B:3C:4D:5F",


      "image": "path/to/image2.png"

    },


  ];

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private languageService: LanguageService,
  ) {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });
    
    // Subscribe to language changes to update column captions
    this.languageService.currentLang$.subscribe(() => {
      this.updateColumnCaptions();
    });
  }

  ngOnInit(): void {
    // Initialize column captions
    this.updateColumnCaptions();
    
    this.route.queryParams.subscribe(params => {
      this.groupId = params['id'];
      if (this.groupId) {
        this.getAccessGroupDetails(this.groupId);
      }
    });
  }

  getAccessGroupDetails(id: number): void {
    const params = { id };

    this.http.get('http://localhost:5273/api/AccessGroups/Get', { params }).subscribe({
      next: (data: any) => {
        this.accessGroup = data.resultData;
      },
      error: (err) => {
        notify(this.languageService.translate('messages.error.loading_access_group_details'), 'error', 2000);
        console.error('Error fetching access group details:', err);
      }
    });
  }

  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }
  
  private updateColumnCaptions() {
    this.columnCaptions = {
      trafficType: this.languageService.translate('access_groups.traffic_type'),
      time: this.languageService.translate('access_groups.time'),
      date: this.languageService.translate('access_groups.date'),
      deviceMacAddress: this.languageService.translate('access_groups.device_mac_address'),
      image: this.languageService.translate('access_groups.image')
    };
  }
}
