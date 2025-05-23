import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Cardsservice } from '../../services/cards/cards.service';
import {
  DxPopupModule,
  DxButtonModule,} from 'devextreme-angular';
@Component({
  selector: 'app-cards',
  standalone: true,
  imports: [CommonModule, DxButtonModule],
  templateUrl: './cards.component.html',
  styleUrl: './cards.component.scss'
})
export class CardsComponent {
  cardsList: any;
  showPopup:boolean= false;
  constructor(private cardsService: Cardsservice) {  }
  ngOnInit() {
    this.getAllCards();
  }
  getAllCards() {
    this.cardsService.getAll('Cards/GetAll').subscribe((data: any) => {
      this.cardsList = data;
      console.log("cardsList", this.cardsList);

    })
  }
  showAddCardsPopup() {
    this.showPopup = true;
  }

}
