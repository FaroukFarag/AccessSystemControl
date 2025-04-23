import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Input, Output, EventEmitter } from '@angular/core';

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

  openPopup(card: any) {
    this.selectedCard = card;
  }

  closePopup() {
    this.selectedCard = null;
  }
}
