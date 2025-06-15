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
export class AccessGroupDevicesComponent  {
  groupId!: number;
  accessGroup: any = null;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private deviceService: DeviceService,
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
      },
      error: (err) => {
        notify('Error fetching access group details', 'error', 2000);
        console.error('Error fetching access group details:', err);
      }
    });
  }

}
