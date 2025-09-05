import { Component, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import {Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { UserService } from '../../../services/users/user.service';
import { Cardsservice } from '../../../services/cards/cards.service';
import { TranslatePipe } from '../../../pipes/translate.pipe';
import { LanguageService } from '../../../services/language/language.service';
import { BackButtonComponent } from '../../../shared/components/back-button/back-button.component';
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
import { DateUtilService } from '../../../services/shared/utils/date-util.service';


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
    DxFormModule,
    BackButtonComponent
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
  cardSettingsPopupVisible: boolean = false;
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
  subscriptionTypes = [
    {
      'id': 1,
      'name': 'Standard'
    },
    {
      'id': 2,
      'name': 'Premium'
    },
    {
      'id': 3,
      'name': 'Enterprise'
    },
  ];

  unit = {
    name: 'Unit name',
    owner: 'Ahmed Adly',
    group: 'group name',
    image: 'assets/images/beach.jpg',
    count:'3'
  };

  cards: any[] = [];

  subscription = {
    plan: 'Standard',
    payment: 1000,
    start: '31-03-2024',
    end: '31-03-2026'
  };


  selectedCard: any = null;
  ownerId: string = '';
  ownerDetails: any;
  cardSettingsPopupTitle: string = '';

  direction: 'ltr' | 'rtl' = 'ltr';

  constructor(private location: Location,
    private route: ActivatedRoute,
    private userService: UserService,
    private cardsService: Cardsservice,
    private http: HttpClient,
    private dateUtil: DateUtilService,
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
        // Load unit cards after getting owner details
        this.loadUnitCards();
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify(this.languageService.translate('messages.error.loading_owner_details'), 'error', 2000);
      }
    });
  }

  loadUnitCards() {
    // Get the first unit ID from owner details, or use a default unit ID
    const unitId = this.ownerDetails?.units?.[0]?.id || 12; // Default to 12 as mentioned in the API
    
    this.cardsService.getAll(`Cards/GetUnitCards?unitId=${unitId}`).subscribe({
      next: (data: any) => {
        this.cards = data.resultData || data || [];
        console.log('Unit Cards loaded:', this.cards);
        console.log('First card structure:', this.cards[0]);
        if (this.cards.length > 0) {
          console.log('Card properties:', Object.keys(this.cards[0]));
        }
      },
      error: (err) => {
        console.error('Error fetching unit cards', err);
        notify(this.languageService.translate('messages.error.loading_unit_cards'), 'error', 2000);
        // Keep the default cards if API fails
        this.cards = [
          { name: 'Card Name', status: 'Active' },
          { name: 'Card Name', status: 'Active' },
          { name: 'Card Name', status: 'Disabled' }
        ];
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

    const startFormatted = this.dateUtil.formatDate(start);

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

  // Card Settings Popup Methods
  showCardSettingsPopup(card: any) {
    console.log('Opening card settings popup for card:', card);
    console.log('Card ID:', card.id);
    console.log('Card name:', card.name);
    console.log('Card status:', card.status);
    
    this.selectedCard = card;
    this.cardSettingsPopupTitle = `${card.name} - ${this.languageService.translate('owners.settings')}`;
    this.cardSettingsPopupVisible = true;
  }

  regenerateCard() {
    if (this.selectedCard) {
      // Get userId from the card data (from GetUnitCards response)
      const userId = this.selectedCard.userId || 
                    this.selectedCard.user_id || 
                    this.selectedCard.UserId ||
                    this.selectedCard.User_Id;
      
      // Get mobile and email from the card data
      const mobile = this.selectedCard.mobile || 
                    this.selectedCard.phoneNumber || 
                    this.selectedCard.phone_number ||
                    this.selectedCard.Mobile ||
                    this.selectedCard.PhoneNumber;
      
      const email = this.selectedCard.email || 
                   this.selectedCard.Email;
      
      console.log('Regenerating card with userId:', userId);
      console.log('Using mobile from card:', mobile);
      console.log('Using email from card:', email);
      console.log('Card properties:', Object.keys(this.selectedCard));
      
      if (userId) {
        const regenerateCardData = {
          userId: userId,
          mobile: mobile || '',
          email: email || ''
        };
        
        console.log('Sending regenerate card data:', regenerateCardData);
        
        this.http.put(`${environment.apiUrl}/Cards/RegenerateCard`, regenerateCardData).subscribe({
          next: (response: any) => {
            notify(this.languageService.translate('owners.card_regenerated'), 'success', 1500);
            this.cardSettingsPopupVisible = false;
            // Refresh the cards list after regenerating
            this.loadUnitCards();
            console.log('Card regenerated successfully:', response);
          },
          error: (err) => {
            console.error('Error regenerating card:', err);
            notify(this.languageService.translate('messages.error.regenerating_card'), 'error', 2000);
          }
        });
      } else {
        console.error('User ID not found for regenerating card');
        notify('User ID not found', 'error', 2000);
      }
    }
  }

  pauseCard() {
    if (this.selectedCard) {
      // Try different possible ID field names for card ID
      const cardId = this.selectedCard.id || 
                    this.selectedCard.cardId || 
                    this.selectedCard.card_id || 
                    this.selectedCard.Id ||
                    this.selectedCard.CardId;
      
      // Get userId from the card data (from GetUnitCards response)
      const userId = this.selectedCard.userId || 
                    this.selectedCard.user_id || 
                    this.selectedCard.UserId ||
                    this.selectedCard.User_Id;
      
      console.log('Pausing card with cardId:', cardId);
      console.log('Using userId from card:', userId);
      console.log('Card properties:', Object.keys(this.selectedCard));
      
      if (userId) {
        // Make API call to pause the card
        const pauseCardData = {
          userId: userId
        };
        
        console.log('Sending pause card data:', pauseCardData);
        
        this.http.delete(`${environment.apiUrl}/Cards/PauseCard`, { body: pauseCardData }).subscribe({
          next: (response: any) => {
            notify(this.languageService.translate('owners.card_paused'), 'warning', 1500);
            this.cardSettingsPopupVisible = false;
            // Refresh the cards list after pausing
            this.loadUnitCards();
            console.log('Card paused successfully:', response);
          },
          error: (err) => {
            console.error('Error pausing card:', err);
            notify(this.languageService.translate('messages.error.pausing_card'), 'error', 2000);
          }
        });
      } else {
        console.error('User ID not found for pausing card');
        notify('User ID not found', 'error', 2000);
      }
    }
  }

  enableCard() {
    if (this.selectedCard) {
      // Try different possible ID field names for card ID
      const cardId = this.selectedCard.id || 
                    this.selectedCard.cardId || 
                    this.selectedCard.card_id || 
                    this.selectedCard.Id ||
                    this.selectedCard.CardId;
      
      // Get userId from the card data (from GetUnitCards response)
      const userId = this.selectedCard.userId || 
                    this.selectedCard.user_id || 
                    this.selectedCard.UserId ||
                    this.selectedCard.User_Id;
      
      // Get mobile and email from the card data
      const mobile = this.selectedCard.mobile || 
                    this.selectedCard.phoneNumber || 
                    this.selectedCard.phone_number ||
                    this.selectedCard.Mobile ||
                    this.selectedCard.PhoneNumber;
      
      const email = this.selectedCard.email || 
                   this.selectedCard.Email;
      
      console.log('Enabling card with ID:', cardId);
      console.log('Card properties:', Object.keys(this.selectedCard));
      console.log('Using userId from card:', userId);
      console.log('Using mobile from card:', mobile);
      console.log('Using email from card:', email);
      
      const enableCardData = {
        userId: userId || 0,
        mobile: mobile || '',
        email: email || ''
      };

      console.log('Sending enable card data:', enableCardData);

      this.http.put(`${environment.apiUrl}/Cards/EnableCard`, enableCardData).subscribe({
        next: (response: any) => {
          notify(this.languageService.translate('owners.card_enabled'), 'success', 1500);
          this.cardSettingsPopupVisible = false;
          // Refresh the cards list after enabling
          this.loadUnitCards();
          console.log('Card enabled successfully:', response);
        },
        error: (err) => {
          console.error('Error enabling card:', err);
          notify(this.languageService.translate('messages.error.enabling_card'), 'error', 2000);
        }
      });
    }
  }

  deleteCard() {
    console.log('Delete card clicked, selectedCard:', this.selectedCard);
    
    if (this.selectedCard) {
      // Try different possible ID field names
      const cardId = this.selectedCard.id || 
                    this.selectedCard.cardId || 
                    this.selectedCard.card_id || 
                    this.selectedCard.Id ||
                    this.selectedCard.CardId;
      
      console.log('Available card properties:', Object.keys(this.selectedCard));
      console.log('Trying to find card ID:', cardId);
      
      if (cardId) {
        const deleteUrl = `${environment.apiUrl}/Cards/Delete?id=${cardId}`;
        console.log('Making DELETE request to:', deleteUrl);
        
        this.http.delete(deleteUrl).subscribe({
          next: (response: any) => {
            console.log('Card deleted successfully:', response);
            notify(this.languageService.translate('owners.card_deleted'), 'success', 1500);
            this.cardSettingsPopupVisible = false;
            // Refresh the cards list after deleting
            this.loadUnitCards();
          },
          error: (err) => {
            console.error('Error deleting card - Full error:', err);
            console.error('Error status:', err.status);
            console.error('Error message:', err.message);
            console.error('Error body:', err.error);
            
            // Show more specific error message
            let errorMessage = this.languageService.translate('messages.error.deleting_card');
            if (err.status === 404) {
              errorMessage = 'Card not found';
            } else if (err.status === 400) {
              errorMessage = 'Invalid card ID';
            } else if (err.status === 500) {
              errorMessage = 'Server error occurred';
            } else if (err.error && err.error.message) {
              errorMessage = err.error.message;
            }
            
            notify(errorMessage, 'error', 3000);
          }
        });
      } else {
        console.error('No card ID found. Available properties:', Object.keys(this.selectedCard));
        console.error('Card object:', this.selectedCard);
        notify('Card ID not found. Available properties: ' + Object.keys(this.selectedCard).join(', '), 'error', 3000);
      }
    } else {
      console.error('No card selected');
      notify('No card selected', 'error', 2000);
    }
  }
}
