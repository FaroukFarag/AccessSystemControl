import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessGroupDevicesComponent } from './access-group-devices.component';

describe('AccessGroupDevicesComponent', () => {
  let component: AccessGroupDevicesComponent;
  let fixture: ComponentFixture<AccessGroupDevicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessGroupDevicesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccessGroupDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
