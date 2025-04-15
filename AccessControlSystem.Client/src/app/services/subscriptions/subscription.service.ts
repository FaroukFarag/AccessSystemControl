import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Subscription } from '../../models/subscriptions/subscription';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService extends BaseService<Subscription>{
  constructor() { 
    super();
  }
}
