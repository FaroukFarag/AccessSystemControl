import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubAdminDashboardComponent } from './sub-admin-dashboard.component';

describe('SubAdminDashboardComponent', () => {
  let component: SubAdminDashboardComponent;
  let fixture: ComponentFixture<SubAdminDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubAdminDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SubAdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
