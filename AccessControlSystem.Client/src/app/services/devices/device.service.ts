import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Device } from '../../models/devices/device';

@Injectable({
  providedIn: 'root'
})
export class DeviceService extends BaseService<Device>{
  constructor() { 
    super();
  }
}
