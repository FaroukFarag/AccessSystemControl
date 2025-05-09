import { TestBed } from '@angular/core/testing';

import { AccessGroupservice } from './access-groups.service';

describe('AccessGroupservice', () => {
  let service: AccessGroupservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccessGroupservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
