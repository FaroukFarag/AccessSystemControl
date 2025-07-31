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
  sortBy = [
    { text: 'Owner Name (A-Z)', value: 'usernameAsc' },
    { text: 'Owner Name (Z-A)', value: 'usernameDesc' },
    { text: 'Phone (Ascending)', value: 'phoneAsc' },
    { text: 'Phone (Descending)', value: 'phoneDesc' }
  ];

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
      this.owners = data.resultData;
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



  navigateToDetailsPage(ownerId: string) {
    this.router.navigate(['/owner-details'], { queryParams: { id: ownerId } });
  }

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
  const selected = e.itemData.value;

  switch (selected) {
    case 'usernameAsc':
      this.owners.sort((a: any, b: any) => a.userName.localeCompare(b.userName));
      break;
    case 'usernameDesc':
      this.owners.sort((a: any, b: any) => b.userName.localeCompare(a.userName));
      break;
    case 'phoneAsc':
      this.owners.sort((a: any, b: any) => a.phoneNumber.localeCompare(b.phoneNumber));
      break;
    case 'phoneDesc':
      this.owners.sort((a: any, b: any) => b.phoneNumber.localeCompare(a.phoneNumber));
      break;
  }

  notify(`Sorted by: ${e.itemData.text}`, 'success', 800);
}


}
