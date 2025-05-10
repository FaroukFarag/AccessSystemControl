import { TestBed } from '@angular/core/testing';

import { Cardsservice } from './cards.service';

describe('AccessGroupservice', () => {
  let service: Cardsservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Cardsservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
