import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessGroupsComponent } from './access-groups.component';

describe('AccessGroupsComponent', () => {
  let component: AccessGroupsComponent;
  let fixture: ComponentFixture<AccessGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessGroupsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccessGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
