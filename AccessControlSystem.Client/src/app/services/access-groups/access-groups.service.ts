import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { AccessGroups } from '../../models/access-groups/access-groups';

@Injectable({
  providedIn: 'root'
})
export class AccessGroupservice extends BaseService<AccessGroups>{
  constructor() { 
    super();
  }

}
