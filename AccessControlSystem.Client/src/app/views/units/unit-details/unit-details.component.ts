import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UnitService } from '../../../services/units/unit.service';
import notify from 'devextreme/ui/notify';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxFormModule,
} from 'devextreme-angular';
import { UserService } from '../../../services/users/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslatePipe } from '../../../pipes/translate.pipe';

@Component({
  selector: 'app-unit-details',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxFormModule,
    TranslatePipe],
  templateUrl: './unit-details.component.html',
  styleUrl: './unit-details.component.scss'
})
export class UnitDetailsComponent {
  unitId: any;
  unitDetails: any = null;
  assignToOwner_popupVisible = false;
  ownersList: any;
  selectedOwnerId: number | null = null;
  formModel = {
    ownerId: null,
    siteId: null
  };
  sites: any[] = [];
  schedules: any[] = [];
  groupDevice_popupVisible: boolean = false;
  groupName: string = '';
  selectedSiteId!: number;
  selectedScheduleId: string = '';
  userRole: any;
  formSubmitted = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private unitsService: UnitService,
    private userService: UserService,
  ) { }
  ngOnInit() {

    this.getAllSites();
    this.getAllSchedules();

    this.userRole = localStorage.getItem('userRole');

    this.route.queryParams.subscribe(params => {
      this.unitId = params['id'];
      console.log('Unit ID:', this.unitId);
      if (this.unitId) {
        this.getUnitDetails(this.unitId);
      }
    });

  }

  getAllSites() {
    this.unitsService.getAll('AirfobSites/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.sites = data.resultData.sites;

      else
        notify('Error getting sites', 'error', 2000);
    })
  }

  getAllSchedules() {
    this.unitsService.getAll('AirfobSchedules/GetAll').subscribe((data: any) => {
      if (data.succeeded)
        this.schedules = data.resultData.schedules;

      else
        notify('Error getting schedules', 'error', 2000);
    })
  }

  getUnitDetails(id: string) {
    this.unitsService.getById('Units/Get', id).subscribe({
      next: (data: any) => {
        this.unitDetails = data.resultData;
        console.log('Unit Details:', this.unitDetails);
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }
  openAssignToOwnerPopup() {
    this.assignToOwner_popupVisible = true;
    this.getAllOwners();
  }

  getAllOwners() {
    this.userService.getAll('Users/GetAllOwners').subscribe((data: any) => {
      this.ownersList = data.resultData;
      console.log("subscriptionssList", this.ownersList);
    })
  }


  submitInits() {
    if (!this.formModel.ownerId || !this.unitId) {
      notify('Please select an owner before submitting.', 'warning', 2000);
      return;
    }

    if (!this.formModel.siteId || !this.unitId) {
      notify('Please select an owner before submitting.', 'warning', 2000);
      return;
    }

    const payload = {
      ownerId: this.formModel.ownerId,
      unitId: Number(this.unitId),
      siteId: this.formModel.siteId
    };

    this.unitsService.update('Units/AssignOwnerToUnit', payload as any).subscribe({
      next: (data: any) => {
        if (data.succeeded) {
          notify('Owner assigned successfully!', 'success', 2000);
          this.assignToOwner_popupVisible = false;
          this.getUnitDetails(this.unitId); // Refresh unit details
        }

        else {
          console.error('Failed to assign owner:', data.message);
          notify('Failed to assign owner', 'error', 2000);
        }
      },
      error: (err) => {
        console.error('Failed to assign owner:', err);
        notify('Failed to assign owner', 'error', 2000);
      }
    });
  }
  navigateToGroup(groupId: number) {
    // Example: navigate to group details page
    this.router.navigate(['/access-groups-devices'], { queryParams: { id: groupId } });

  }

  navigateToAssignedOwners(groupId: number) {
    // Navigate to owners page with group filter
    this.router.navigate(['/owners'], { queryParams: { groupId: groupId } });
  }
}
