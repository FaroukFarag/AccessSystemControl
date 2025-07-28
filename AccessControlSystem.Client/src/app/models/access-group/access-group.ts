
import { Device } from '../devices/device';
export interface AccessGroup {
  name: string;
  siteId: number;
  scheduleId: number;
  devices: string[];
}

