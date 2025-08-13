import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Router } from '@angular/router';
import { LanguageService } from '../../../services/language/language.service';
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
  direction: 'ltr' | 'rtl' = 'ltr';
  sortBy = ['Recent', 'Name'];

  popupVisible: boolean = false;
  ownerData: User = {
    id: 0,
    userName: '',
    email: '',
    phoneNumber: '',
    roleId: '3',
    password: '',
    confirmPassword: '', 
    subscriptionId: +localStorage.getItem('subscriptionId')!
  };
  owners: any;

  constructor(
    private router: Router,
    private userService: UserService,
    private location: Location,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.getAllOwners();
  }
  backClicked() {
    this.location.back();
  }

  getAllOwners(orderBy?: string): void {
    const baseUrl = 'Users/GetAllOwners';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.userService.getAll(url).subscribe({
      next: (data: any) => {
        this.owners = data.resultData;
      },
      error: (err) => console.error("Failed to load owners:", err)
    });
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
      confirmPassword: '',
      subscriptionId: +localStorage.getItem('subscriptionId')!
    };
  }

  submitOwner() {
    const result = this.dxForm.instance.validate();

    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    this.userService.create('Users/Create', this.ownerData).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify('Device created successfully', 'success', 1500);
          this.popupVisible = false;
          this.getAllOwners();
        }

        else {
          notify('Error creating device', 'error', 2000);
          console.error(response.message);
        }
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
    this.getAllOwners(e.itemData);
  }


}
