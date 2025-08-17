import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Subscription } from '../../models/subscriptions/subscription';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService extends BaseService<Subscription>{
  constructor() { 
    super();
  }

  createWithImage(endpoint: string, formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/${endpoint}`, formData);
  }
  
  updateWithImage(endpoint: string, formData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/${endpoint}`, formData);
  }

  upgradeSubscription(endpoint: string, data: any): Observable<any> {
    return this.http.patch(`${this.baseUrl}/${endpoint}`, data);
  }
}
