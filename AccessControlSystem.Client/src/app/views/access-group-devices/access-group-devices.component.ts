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
export class AccessGroupDevicesComponent  {
  groupId!: number;
  accessGroup: any = null;
  devices: any;
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private router: Router,
) { }

  ngOnInit(): void {
   

     this.route.queryParams.subscribe(params => {
       this.groupId = params['id'];
       if (this.groupId) {
         this.getAccessGroupDetails(this.groupId);      }
    });

  }

  getAccessGroupDetails(id: number): void {
    const params = { id };

    this.http.get('https://localhost:7096/api/AccessGroups/Get', { params }).subscribe({
      next: (data: any) => {
        this.accessGroup = data;
        console.log('Access group details:', this.accessGroup);
        this.displayDevices(this.accessGroup.devices); // Call to display devices
      },
      error: (err) => {
        notify('Error fetching access group details', 'error', 2000);
        console.error('Error fetching access group details:', err);
      }
    });
  }


  displayDevices(devices: any[]): void {
    // Logic to display devices in the template
    console.log('Devices in this group:', devices);
    // You can set a property to hold these devices and use it in the template
    this.devices = devices; // Assuming you have a devices property in your component
  }


  navigateToDetailsPage(deviceId: string) {
    this.router.navigate(['/device-details'], { queryParams: { id: deviceId } });
  }


}
