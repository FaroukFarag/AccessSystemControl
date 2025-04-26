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
  UnitsData = {
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
    SubscriptionId: '1'
   
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

  units: any = [
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



  ngOnInit() {
    this.getAllSubscriptions();
  }

  getAllSubscriptions() {
    this.unitsService.getAll('Units/GetAll').subscribe((data: any) => {
      this.unitsList = data;
      console.log("UnitsList", this.unitsList);

    })
  }

  showAddDevicePopup() {
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
      SubscriptionId: '1'
    };
    this.imageValidationError = '';
    this.popupVisible = true;
  }
  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
  navigateToDetailsPage() {
    this.router.navigate(['/unit-details']);
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
    if (!this.UnitsData.unitImageFile) {
      this.imageValidationError = 'Image is required';
      return;
    }

    const result = this.dxForm.instance.validate();
    if (!result.isValid) {
      notify('Please fill in all required fields.', 'warning', 1500);
      return;
    }

    const formData = new FormData();
    formData.append('ImageFile', this.UnitsData.unitImageFile || '');
    formData.append('ImageEncode', this.UnitsData.unitImageUrl || '');
    formData.append('Name', this.UnitsData.Name);
    formData.append('Number', this.UnitsData.Number);
    formData.append('Area', this.UnitsData.Area);
    formData.append('CardNumber', this.UnitsData.CardNumber);

    formData.append('UserId', this.UnitsData.UserId);
    formData.append('SubscriptionId', this.UnitsData.SubscriptionId);

    const devices = Array.isArray(this.UnitsData.AccessGroupDevices)
      ? this.UnitsData.AccessGroupDevices
      : [];

    devices.forEach((deviceId: number, index: number) => {
      formData.append(`AccessGroupDevices[${index}]`, deviceId.toString());
    });

    formData.append('ImagePath', this.UnitsData.ImagePath);
    formData.append('ImageEncode', this.UnitsData.ImageEncode);
    formData.append('Id', this.UnitsData.Id);
    formData.append('ImageFile', this.UnitsData.unitImageFile);

    this.unitsService.create('Units/Create', formData as any).subscribe({
      next: (response) => {
        notify('Device created successfully', 'success', 1500);
        this.popupVisible = false;
        this.getAllSubscriptions();
      },
      error: (err) => {
        notify('Error creating device', 'error', 2000);
        console.error(err);
      }
    });
  }


}
