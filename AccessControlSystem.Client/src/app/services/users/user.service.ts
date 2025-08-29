import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { User } from '../../models/users/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService<User>{
  constructor() { 
    super();
  }

  upgradeSubscription(endpoint: string, data: any): Observable<any> {
    return this.http.patch(`${this.baseUrl}/${endpoint}`, data);
  }

  // Generic POST for actions that are not full User payloads (e.g., cancel/delete by id)
  postAction(endpoint: string, data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/${endpoint}`, data);
  }
}
