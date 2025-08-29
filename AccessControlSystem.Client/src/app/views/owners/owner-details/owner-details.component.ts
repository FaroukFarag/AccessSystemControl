import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/users/user.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { LanguageService } from '../../../services/language/language.service';
import notify from 'devextreme/ui/notify';
import {
  DxPopupModule,
  DxButtonModule,
  DxTemplateModule,
  DxToolbarModule,
  DxSelectBoxModule,
  DxTextAreaModule,
  DxDateBoxModule,
  DxNumberBoxModule,
  DxFormModule,
} from 'devextreme-angular';
import { DxFormComponent } from 'devextreme-angular';


@Component({
  selector: 'app-owner-details',
  standalone: true,
  imports: [
    CommonModule, 
    TranslatePipe,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxNumberBoxModule,
    DxFormModule
  ],
  templateUrl: './owner-details.component.html',
  styleUrl: './owner-details.component.scss'
})
export class OwnerDetailsComponent {
  @ViewChild('upgradeForm', { static: false }) upgradeForm!: DxFormComponent;
  
  groupDevices = new Array(5).fill({});
  allDevices = new Array(7).fill({});
  
  // Popup properties
  upgradePopupVisible: boolean = false;
  cancelConfirmationPopupVisible: boolean = false;
  isCancelling: boolean = false;
  isUpgrading: boolean = false;
  
  // Data for upgrade popup
  upgradeData = {
    id: 0,
    subscriptionType: 0,
    startDate: '',
    numberOfMonths: 1
  };
  
  // Subscription types for dropdown
  subscriptionTypes: any[] = [];

  unit = {
    name: 'Unit name',
    owner: 'Ahmed Adly',
    group: 'group name',
    image: 'assets/images/beach.jpg',
    count:'3'
  };

  cards: any[] = [
    { name: 'Card Name', status: 'Active' },
    { name: 'Card Name', status: 'Active' },
    { name: 'Card Name', status: 'Disabled' }
  ];

  subscription = {
    plan: 'Standard',
    payment: 1000,
    start: '31-03-2024',
    end: '31-03-2026'
  };


  selectedCard: any = null;
  ownerId: string = '';
  ownerDetails: any;

  direction: 'ltr' | 'rtl' = 'ltr';

  constructor(private location: Location,
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private languageService: LanguageService,
) {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });
  }
  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.ownerId = params['id'];
      if (this.ownerId) {
        this.getOwnerDetails(this.ownerId);
      }
    });
  }

  getOwnerDetails(id: string) {
    this.userService.getById('Users/GetOwnerDetails', id).subscribe({
      next: (data: any) => {
        this.ownerDetails = data.resultData;
        console.log('Owner Details:', this.ownerDetails);
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify(this.languageService.translate('messages.error.loading_owner_details'), 'error', 2000);
      }
    });

  }
  openPopup(card: any) {
    this.selectedCard = card;
  }

  closePopup() {
    this.selectedCard = null;
  }
  goBack() {
    this.location.back();
  }
  navigateToUnitDetailsPage(unitId: number) {
    this.router.navigate(['/unit-details'], { queryParams: { id: unitId } });
  }

  showUpgradePopup() {
    this.upgradePopupVisible = true;
    this.upgradeData = {
      id: this.ownerId ? parseInt(this.ownerId) : 0,
      subscriptionType: 0,
      startDate: '',
      numberOfMonths: 1
    };
  }

  submitUpgrade() {
    const result = this.upgradeForm.instance.validate();
    if (!result.isValid) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    this.isUpgrading = true;

    const start = new Date(this.upgradeData.startDate);

    if (isNaN(start.getTime())) {
      notify('Invalid start date', 'error', 2000);
      return;
    }

    const startFormatted = start.toISOString().split('T')[0];

    this.upgradeData.startDate = startFormatted;

    // TODO: Replace with actual service call
    this.userService.postAction('Users/UpgradeSubscription', this.upgradeData as any).subscribe({
      next: (response: any) => {
        console.log('API Response received, setting isUpgrading to false');
        this.isUpgrading = false;
        if (response.succeeded) {
          notify(this.languageService.translate('validation.subscription_updated'), 'success', 1500);
          this.upgradePopupVisible = false;

          // Refresh owner data
          if (this.ownerId) {
            this.getOwnerDetails(this.ownerId);
          }
        } else {
          notify(response.message || this.languageService.translate('validation.subscription_update_error'), 'error', 2000);
        }
      },
      error: (err) => {
        console.log('API Error received, setting isUpgrading to false');
        this.isUpgrading = false;
        notify(this.languageService.translate('validation.subscription_update_error'), 'error', 2000);
        console.error(err);
      }
    });
  }

  showCancelConfirmationPopup() {
    this.cancelConfirmationPopupVisible = true;
  }

  confirmCancelSubscription() {
    this.isCancelling = true;

    console.log('Attempting to cancel subscription for owner ID:', this.ownerId);

    // TODO: Replace with actual service call
    this.userService.delete(`Users/CancelSubscription?id=${this.ownerId}`).subscribe({
      next: (response: any) => {
        this.isCancelling = false;
        console.log('Cancel response:', response);
        if (response.succeeded) {
          notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancelled_successfully'), 'success', 2000);
          this.cancelConfirmationPopupVisible = false;
          // Navigate back to owners list
          this.router.navigate(['/owners']);
        } else {
          notify(response.message || this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
        }
      },
      error: (err) => {
        this.isCancelling = false;
        console.error('Error cancelling subscription:', err);
        notify(this.languageService.translate('subscriptions.subscription_details.subscription_cancel_error'), 'error', 2000);
      }
    });
  }

  getProgress(used: number, total: number): number {
    if (total === 0) return 0;
    return Math.round((used / total) * 100);
  }
}
