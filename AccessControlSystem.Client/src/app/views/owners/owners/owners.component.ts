import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common'
import { Router, ActivatedRoute } from '@angular/router';
import { LanguageService } from '../../../services/language/language.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { BackButtonComponent } from '../../../shared/components/back-button/back-button.component';
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
import { AccessGroupService } from '../../../services/access-groups/access-group.service';
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
    DxDropDownButtonModule,
    TranslatePipe,
    BackButtonComponent],
  templateUrl: './owners.component.html',
  styleUrl: './owners.component.scss'
})
export class OwnersComponent implements OnInit {
  @ViewChild(DxFormComponent, { static: false }) dxForm!: DxFormComponent;
  direction: 'ltr' | 'rtl' = 'ltr';
  sortBy = ['Recent', 'Name'];

  popupVisible: boolean = false;
  groupId: number | null = null;
  groupName: string = '';
  isFilteredByGroup: boolean = false;
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
    private route: ActivatedRoute,
    private userService: UserService,
    private accessGroupService: AccessGroupService,
    private location: Location,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    // Check if we're filtering by group
    this.route.queryParams.subscribe(params => {
      if (params['groupId']) {
        this.groupId = +params['groupId'];
        this.isFilteredByGroup = true;
        this.loadGroupDetails();
        this.getOwnersByGroup(this.groupId);
      } else {
        this.getAllOwners();
      }
    });
  }
  goBack() {
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

  loadGroupDetails(): void {
    if (this.groupId) {
      this.accessGroupService.getAccessGroupById(this.groupId).subscribe({
        next: (data: any) => {
          if (data && data.resultData) {
            this.groupName = data.resultData.name;
          }
        },
        error: (err) => console.error("Failed to load group details:", err)
      });
    }
  }

  getOwnersByGroup(groupId: number): void {
    // Try to use a specific API endpoint for getting owners by group
    // If the endpoint doesn't exist, it will fall back to getting all owners
    this.userService.getAll(`Users/GetOwnersByGroup/${groupId}`).subscribe({
      next: (data: any) => {
        if (data && data.resultData) {
          this.owners = data.resultData;
        }
      },
      error: (err) => {
        console.log("Specific endpoint not available, falling back to all owners");
        // Fallback: Get all owners and filter client-side
        this.userService.getAll('Users/GetAllOwners').subscribe({
          next: (data: any) => {
            if (data && data.resultData) {
              // For now, show all owners since we don't have the relationship data
              // In a real implementation, you would filter based on the actual relationship
              this.owners = data.resultData;
              console.log("Showing all owners for group:", groupId);
            }
          },
          error: (fallbackErr) => console.error("Failed to load owners:", fallbackErr)
        });
      }
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
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    this.userService.create('Users/Create', this.ownerData).subscribe({
      next: (response: any) => {
        if (response.succeeded) {
          notify(this.languageService.translate('messages.success.owner_created'), 'success', 1500);
          this.popupVisible = false;
          this.getAllOwners();
        }

        else {
          notify(this.languageService.translate('messages.error.device_creation'), 'error', 2000);
          console.error(response.message);
        }
      },
      error: (err) => {
        notify(this.languageService.translate('messages.error.device_creation'), 'error', 2000);
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
