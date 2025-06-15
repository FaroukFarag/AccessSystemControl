export interface AccessGroupDevice {
  id: number;
  imagePath: string;
  imageFile: File | null;
  name: string;
  macAddress: string;
  deviceType: number | string;
  deviceTypeName: string;
  active: boolean;
  subscriptionId: number;
}


import { Device } from '../devices/device';

export interface AccessGroup {
  id: number;
  name: string;
  devices: Device[];
}

