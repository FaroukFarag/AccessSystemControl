import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Unit } from '../../models/units/unit';

@Injectable({
  providedIn: 'root'
})
export class UnitService extends BaseService<Unit>{
  constructor() { 
    super();
  }
}
