import { Component, ViewChild, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common'
import { Router } from '@angular/router';
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
  DxTagBoxModule,
  DxFormModule,
  DxFileUploaderModule,
} from 'devextreme-angular';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';

import notify from 'devextreme/ui/notify';
import { DomSanitizer } from '@angular/platform-browser';
import { UnitService } from '../../../services/units/unit.service';
import { AccessGroup } from '../../../models/access-group/access-group';
import { AccessGroupService } from '../../../services/access-groups/access-group.service';


@Component({
  selector: 'app-units',
  standalone: true,
  imports: [CommonModule,
    TranslatePipe,
    DxPopupModule,
    DxButtonModule,
    DxTemplateModule,
    DxToolbarModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxDateBoxModule,
    DxFormModule,
    DxDropDownButtonModule,
    DxFileUploaderModule,
    DxTagBoxModule,
    BackButtonComponent], templateUrl: './units.component.html',
  styleUrl: './units.component.scss'
})
export class UnitsComponent implements OnInit {
  @ViewChild('unitFormRef', { static: false }) dxForm: any;
  direction: 'ltr' | 'rtl' = 'ltr';
  popupVisible: boolean = false;
  //sortBy = ['Recent', 'date'];
  sortBy = ['Recent', 'Name'];

  unitsList: any;
  imageValidationError: string = '';
  subscriptionId: any;
  unitsData = {
    unitImageFile: null,
    unitImageUrl: '',
    Name: '',
    Number: '',
    Area: '',
    CardNumber: '',
    AccessGroups: [],
    ImageEncode: '',
    ImageFile: null,
    ImagePath: '',
    Id: '0',
    UserId: '1',
    SubscriptionId: '0',
  };
  deviceTypeEditorOptions: any
  subscriptionTypes = [
    {
      'id': '1',
      'name': 'Standard'
    },
    {
      'id': '2',
      'name': 'Premium'
    },
    {
      'id': '3',
      'name': 'Enterprise'
    },
  ]
  areasList = [
    { id: 1, name: 'Area 1' },
    { id: 2, name: 'Area 2' },
    { id: 3, name: 'Area 3' }
  ];
  availableDevices = [
    { id: 1, name: 'Group A' },
    { id: 2, name: 'Group B' },
    { id: 3, name: 'Group C' },
    { id: 4, name: 'Group D' },
  ];
  accessGroups: AccessGroup[] = [];
  fileUploaderValue: any[] = [];

  constructor(
    private router: Router,
    private unitsService: UnitService,
    private accessGroupService: AccessGroupService,
    private sanitizer: DomSanitizer,
    private languageService: LanguageService,
    private location: Location) {
    this.deviceTypeEditorOptions = {
      dataSource: this.subscriptionTypes,
      valueExpr: 'name',
      displayExpr: 'name',
      searchEnabled: true,
      showClearButton: true,
      value: 'Type 1',
      placeholder: 'Device Access'
    };

  }

  ngOnInit() {
    // Subscribe to direction changes
    this.languageService.direction$.subscribe(direction => {
      this.direction = direction;
    });

    this.subscriptionId = localStorage.getItem('subscriptionId');

    this.unitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroups: [],
      ImageEncode: '',
      ImageFile: null,
      ImagePath: '',
      Id: '0',
      UserId: '1',
      SubscriptionId: this.subscriptionId,
    };

    this.getAllUnits();
    this.getAllAccessGroups();
  }

  getAllUnits(orderBy?: string): void {
    const baseUrl = 'Units/GetAll';
    const url = orderBy?.trim()
      ? `${baseUrl}/${encodeURIComponent(orderBy.trim())}`
      : baseUrl;

    this.unitsService.getAll(url).subscribe({
      next: (data: any) => {
        this.unitsList = data.resultData;
      },
      error: (err) => console.error("Failed to load units:", err)
    });
  }

  getAllAccessGroups() {
    this.unitsService.getAll('AccessGroups/GetAll').subscribe((data: any) => {
      this.accessGroups = data.resultData;

    })
  }

  showAddUnitPopup() {
    this.subscriptionId = localStorage.getItem('subscriptionId');
    
    // Reset file uploader value to force re-render
    this.fileUploaderValue = [];
    
    this.unitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroups: [],
      ImageEncode: '',
      ImageFile: null,
      ImagePath: '',
      Id: '0',
      UserId: '1',
      SubscriptionId: this.subscriptionId
    };
    this.imageValidationError = '';
    this.popupVisible = true;
  }

  onPopupHidden() {
    // Reset unit data when popup is closed
    this.unitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroups: [],
      ImageEncode: '',
      ImageFile: null,
      ImagePath: '',
      Id: '0',
      UserId: '1',
      SubscriptionId: this.subscriptionId
    };
    this.imageValidationError = '';
    this.fileUploaderValue = [];
  }

  navigateToDetailsPage(unitId: number) {
    // this.router.navigate(['/unit-details', { id: unitId }]);
    this.router.navigate(['/unit-details'], { queryParams: { id: unitId } });

  }

  sanitizeImage(image: string) {
    return this.sanitizer.bypassSecurityTrustUrl(image);
  }

  onImageChange(e: any) {
    const file = e.value[0];
    if (file) {
      this.unitsData.unitImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.unitsData.unitImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  getSelectedAccessGroups(selectedAccessGroupIds: number[]): AccessGroup[] {
    return this.accessGroups.filter(ag =>
      selectedAccessGroupIds.includes(ag.id)
    );
  }

  submitInits() {
    // Validate required fields
    if (!this.unitsData.Name || !this.unitsData.Number || !this.unitsData.Area || !this.unitsData.CardNumber || !this.unitsData.SubscriptionId || !this.unitsData.AccessGroups || !this.unitsData.AccessGroups.length) {
      notify(this.languageService.translate('validation.fill_required_fields'), 'warning', 1500);
      return;
    }

    const formData = new FormData();
    const t = this.getSelectedAccessGroups(this.unitsData.AccessGroups);

    formData.append('name', this.unitsData.Name);
    formData.append('number', this.unitsData.Number.toString());
    formData.append('area', this.unitsData.Area.toString());
    formData.append('cardNumber', this.unitsData.CardNumber.toString());
    formData.append('subscriptionId', this.unitsData.SubscriptionId.toString());
    formData.append('AccessGroupsJson', JSON.stringify(t));

    // Check if an image file is selected
    if (this.unitsData.unitImageFile) {
      formData.append('imageFile', this.unitsData.unitImageFile);
    }


    if (this.unitsData.unitImageUrl) {
      formData.append('imagePath', this.unitsData.unitImageUrl);
    }


    console.log('Data being sent to the API:', {
      name: this.unitsData.Name,
      number: this.unitsData.Number,
      area: this.unitsData.Area,
      cardNumber: this.unitsData.CardNumber,
      subscriptionId: this.unitsData.SubscriptionId,
      accessGroups: this.unitsData.AccessGroups,
      imagePath: this.unitsData.unitImageUrl,
      imageFile: this.unitsData.unitImageFile ? this.unitsData.unitImageFile : null
    });


    this.unitsService.create('Units/Create', formData as any).subscribe({
      next: (response) => {
        notify(this.languageService.translate('messages.success.unit_created'), 'success', 1500);
        this.popupVisible = false;
        this.getAllUnits();
      },
      error: (err) => {
        notify(this.languageService.translate('messages.error.unit_creation') + ': ' + (err?.error?.details || err.message), 'error', 2000);
      }
    });
  }

  /*Sorting Function */

  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    this.getAllUnits(e.itemData);
  }

  goBack(): void {
    this.location.back();
  }

}
