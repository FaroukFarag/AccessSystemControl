import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';  // or use a service

@Component({
  selector: 'app-access-group-devices',
  templateUrl: './access-group-devices.component.html',
  styleUrls: ['./access-group-devices.component.scss']
})
export class AccessGroupDevicesComponent implements OnInit {
  devices: any[] = [];
  accessGroupId!: string;

  constructor(private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.accessGroupId = this.route.snapshot.paramMap.get('id')!;
    this.getDevicesByAccessGroupId(this.accessGroupId);
  }

  getDevicesByAccessGroupId(id: string) {
    this.http.get(`/api/AccessGroupsDevices/Get?accessGroupId=${id}`)
      .subscribe((data: any) => {
        this.devices = data;
        console.log('Devices:', this.devices);
      });
  }
}
