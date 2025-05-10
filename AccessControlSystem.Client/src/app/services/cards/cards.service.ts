import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { Cards } from '../../models/cards/cards';

@Injectable({
  providedIn: 'root'
})
export class Cardsservice extends BaseService<Cards>{
  constructor() { 
    super();
  }

}
