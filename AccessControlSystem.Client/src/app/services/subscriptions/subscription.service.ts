import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Subscription } from '../../models/subscriptions/subscription';
import { Observable, tap, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService extends BaseService<Subscription>{
  constructor() { 
    super();
  }

  override getById(endpoint: string, id: number | string): Observable<any> {
    console.log('SubscriptionService: Getting subscription with ID:', id);
    console.log('SubscriptionService: Endpoint:', endpoint);
    console.log('SubscriptionService: Full URL:', `${this.baseUrl}/${endpoint}/${id}`);
    
    return this.http.get<any>(`${this.baseUrl}/${endpoint}/${id}`).pipe(
      tap(response => console.log('SubscriptionService: Response received:', response)),
      catchError(error => {
        console.error('SubscriptionService: Error getting subscription:', error);
        throw error;
      })
    );
  }

  createWithImage(endpoint: string, formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/${endpoint}`, formData);
  }
  
  updateWithImage(endpoint: string, formData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/${endpoint}`, formData);
  }
}
