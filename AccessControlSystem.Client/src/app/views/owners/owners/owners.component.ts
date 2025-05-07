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
  owners: any = [
    {
      'id': '1',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '2',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "15"
    }, {
      'id': '3',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '4',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "24"
    }, {
      'id': '5',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '6',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '7',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '8',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '9',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '10',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '11',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '12',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '13',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '14',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '15',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '16',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '17',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '18',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '19',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '20',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '21',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '22',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '23',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '24',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '25',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    }, {
      'id': '26',
      'name': 'Village Name',
      'subscription': 'Standard',
      'dvices': "20"
    },
  ]

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    this.getAllOwners();
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

  navigateToDetailsPage() {
    this.router.navigate(['/owner-details']);
  }
}
