import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Router } from '@angular/router';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxFormModule,
  DxFormComponent,
} from 'devextreme-angular';
import { DxDropDownButtonModule, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';

import notify from 'devextreme/ui/notify';
import { UserService } from '../../../services/users/user.service';
import { User } from '../../../models/users/user';
import { Location } from '@angular/common';
@Component({
  selector: 'app-owners',
  standalone: true,
  imports: [CommonModule,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxDropDownButtonModule,],
  templateUrl: './owners.component.html',
  styleUrl: './owners.component.scss'
})
export class OwnersComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  sortBy = ['Recent', 'date'];
  popupVisible: boolean = false;
  ownerData: User = {
    id: 0,
    userName: '',
    email: '',
    phoneNumber: '',
    roleId: '3',
    password: '',
    confirmPassword: ''
  };
  owners: any;

  constructor(private router: Router,
    private userService: UserService,
    private location: Location) { }

  ngOnInit(): void {
    this.getAllOwners();
  }
  backClicked() {
    this.location.back();
  }
  getAllOwners() {
    this.userService.getAll('Users/GetAllOwners').subscribe((data: any) => {
      this.owners = data;
    })
  }

  passwordComparison = () => {
    return this.ownerData.password;
  };

  showAddOwnerPopup() {
    this.popupVisible = true;
    this.ownerData = {
      id: 0,
      userName: '',
      email: '',
      phoneNumber: '',
      roleId: '3',
      password: '',
      confirmPassword: ''
    };
  }

  submitOwner() {
    const result = this.dxForm.instance.validate();

    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    this.userService.create('Users/Create', this.ownerData).subscribe({
      next: (response) => {
        notify('Device created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllOwners(); 
      },
      error: (err) => {
        notify('Error creating device', 'error', 2000);
        console.error(err);
      }
    });
  }

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }

  //navigateToDetailsPage() {
  //  this.router.navigate(['/owner-details']);
  //}

  navigateToDetailsPage(ownerId: string) {
    this.router.navigate(['/owner-details'], { queryParams: { id: ownerId } });
  }
}
