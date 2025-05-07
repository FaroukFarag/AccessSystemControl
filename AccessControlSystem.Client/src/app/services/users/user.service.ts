import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { User } from '../../models/users/user';

@Injectable({
  providedIn: 'root'
})
export class UserService  extends BaseService<User>{
  constructor() { 
    super();
  }
}
