import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { AccessGroup } from '../../models/access-group/access-group';

@Injectable({
  providedIn: 'root'
})
export class AccessGroupservice extends BaseService<AccessGroup>{
  constructor() { 
    super();
  }

}
