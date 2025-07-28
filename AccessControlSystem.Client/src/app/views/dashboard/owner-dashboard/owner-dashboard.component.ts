import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxFormModule,
  DxDataGridModule
} from 'devextreme-angular';
import { UserService } from '../../../services/users/user.service';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-owner-dashboard',
  standalone: true,
  imports: [DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxFormModule,
    DxPopupModule,
    DxDataGridModule],
  templateUrl: './owner-dashboard.component.html',
  styleUrl: './owner-dashboard.component.scss'
})
export class OwnerDashboardComponent implements OnInit {
  manageVistors_popupVisible = false;

  formModel = {
    name: '',
    visitDate: new Date(),
    unitId: 1, // Change this to the correct unitId from your app
    subscriptionId: 0
  };

  owner = {
    name: 'Ahmed Adly',
    subscription: 'Premium',
    devices: 3
  };

  devices = [
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' },
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' },
    { name: 'Device name', status: 'Active', mac: '50:B0:0D:63:...', image: 'device.png' }
  ];
  vistorsDetails = [
    {
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },{
      name: 'Device name',
      email: 'vistor@gmail.com',
      phone: '01127257820',
      deviceAccess: 'Group 1',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
    },
  ];
  devicesTraffic = [
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    }, {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '1 year and 2 months'
    },
    {
      name: 'Device name',
      start: 'Sep 04, 2024',
      end: 'Sep 05, 2025',
      remaining: '4 months'
    }
  ];
  ownerDetails: any;
  userId: any;
  constructor(private http: HttpClient,
    private userService: UserService,
) { }

  ngOnInit() {
    const subId = localStorage.getItem('subscriptionId');
    this.formModel.subscriptionId = subId ? +subId : 0;
    this.userId = localStorage.getItem('userId') || '';
    console.log('User ID:', this.userId);
    this.getOwnerDetails(this.userId);
  }

  openManageVistorsPopup() {
    this.manageVistors_popupVisible = true;
  }

  submitVistor() {
    const payload = {
      id: 0,
      name: this.formModel.name,
      visitDate: this.formModel.visitDate,
      unitId: this.formModel.unitId,
      subscriptionId: this.formModel.subscriptionId
    };

    this.http.post('/api/Visitors/Create', payload).subscribe({
      next: (res) => {
        this.manageVistors_popupVisible = false;
      },
      error: (err) => {
        notify('Error creating visitor:', 'error', 2000);

      }
    });
  }


  getOwnerDetails(id: string) {
    this.userService.getById('Users/GetOwnerDetails', this.userId).subscribe({
      next: (data: any) => {
        this.ownerDetails = data;
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }
}
