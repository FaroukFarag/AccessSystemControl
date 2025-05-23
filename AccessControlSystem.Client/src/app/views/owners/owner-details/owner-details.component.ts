import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/users/user.service';
import notify from 'devextreme/ui/notify';


@Component({
  selector: 'app-owner-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './owner-details.component.html',
  styleUrl: './owner-details.component.scss'
})
export class OwnerDetailsComponent {
  groupDevices = new Array(5).fill({});
  allDevices = new Array(7).fill({});

  unit = {
    name: 'Unit name',
    owner: 'Ahmed Adly',
    group: 'group name',
    image: 'assets/images/beach.jpg',
    count:'3'
  };

  cards = [
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

  constructor(private location: Location,
    private route: ActivatedRoute,
    private userService: UserService,
) {

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
    this.userService.getById('Users/Get', id).subscribe({
      next: (data: any) => {
        this.ownerDetails = data;
        console.log('Owner Details:', this.ownerDetails);
      },
      error: (err) => {
        console.error('Error fetching device details', err);
        notify('Error fetching device details', 'error', 2000);
      }
    });

  }
  openPopup(card: any) {
    this.selectedCard = card;
  }

  closePopup() {
    this.selectedCard = null;
  }
  backClicked() {
    this.location.back();
  }
}
