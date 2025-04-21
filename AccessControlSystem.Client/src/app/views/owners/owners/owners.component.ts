import { Component } from '@angular/core';
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
} from 'devextreme-angular';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { DxDropDownButtonModule, DxDropDownButtonComponent, DxDropDownButtonTypes } from 'devextreme-angular/ui/drop-down-button';

import notify from 'devextreme/ui/notify';
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
export class OwnersComponent {
  sortBy = ['Recent', 'date'];
  constructor(private router: Router) { }

  subscriptions: any = [
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


  onItemClick(e: DxDropDownButtonTypes.ItemClickEvent): void {
    notify(e.itemData.name || e.itemData, 'success', 600);
  }
  navigateToDetailsPage() {
    this.router.navigate(['/owner-details']);
  }
}
