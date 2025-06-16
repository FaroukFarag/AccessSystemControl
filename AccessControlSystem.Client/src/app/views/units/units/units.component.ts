import { Component, ViewChild } from '@angular/core';
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
  DxFileUploaderModule,
} from 'devextreme-angular';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';

import notify from 'devextreme/ui/notify';
import { DomSanitizer } from '@angular/platform-browser';
import { UnitService } from '../../../services/units/unit.service';


@Component({
  selector: 'app-units',
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
    DxFileUploaderModule,],  templateUrl: './units.component.html',
  styleUrl: './units.component.scss'
})
export class UnitsComponent {
  @ViewChild('subscriptionFormRef', { static: false }) dxForm: any;
  popupVisible: boolean = false;
  sortBy = ['Recent', 'date'];
  unitsList: any;
  imageValidationError: string = '';
  subscriptionId: any;
  UnitsData: any;
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

  constructor(private router: Router, private unitsService: UnitService, private sanitizer: DomSanitizer) {
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
    this.subscriptionId = localStorage.getItem('subscriptionId');

    this.UnitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroupDevices: [],
      ImageEncode: '',
      ImageFile: null,
      ImagePath: '',
      Id: '0',
      UserId: '1',
      SubscriptionId: this.subscriptionId,
    };

    this.getAllUnits();
  }

  getAllUnits() {
    this.unitsService.getAll('Units/GetAll').subscribe((data: any) => {
      this.unitsList = data;

    })
  }

  showAddDevicePopup() {
    this.subscriptionId = localStorage.getItem('subscriptionId');
    this.UnitsData = {
      unitImageFile: null,
      unitImageUrl: '',
      Name: '',
      Number: '',
      Area: '',
      CardNumber: '',
      AccessGroupDevices: [],
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
  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
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
      this.UnitsData.unitImageFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.UnitsData.unitImageUrl = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  submitInits() {
    // Validate required fields
    if (!this.UnitsData.Name || !this.UnitsData.Number || !this.UnitsData.Area || !this.UnitsData.CardNumber || !this.UnitsData.SubscriptionId) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const formData = new FormData();
    formData.append('name', this.UnitsData.Name);
    formData.append('number', this.UnitsData.Number.toString());
    formData.append('area', this.UnitsData.Area.toString());
    formData.append('cardNumber', this.UnitsData.CardNumber.toString());
    formData.append('subscriptionId', this.UnitsData.SubscriptionId.toString()); 

    // Check if an image file is selected
    if (this.UnitsData.unitImageFile) {
      formData.append('imageFile', this.UnitsData.unitImageFile);
    }

    
    if (this.UnitsData.unitImageUrl) {
      formData.append('imagePath', this.UnitsData.unitImageUrl);
    }

    
    console.log('Data being sent to the API:', {
      name: this.UnitsData.Name,
      number: this.UnitsData.Number,
      area: this.UnitsData.Area,
      cardNumber: this.UnitsData.CardNumber,
      subscriptionId: this.UnitsData.SubscriptionId,
      imagePath: this.UnitsData.unitImageUrl,
      imageFile: this.UnitsData.unitImageFile ? this.UnitsData.unitImageFile.name : null
    });

   
    this.unitsService.create('Units/Create', formData as any).subscribe({
      next: (response) => {
        notify('Unit created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllUnits();
      },
      error: (err) => {
        notify('Error creating Unit: ' + (err?.error?.details || err.message), 'error', 2000);
      }
    });
  }



}
