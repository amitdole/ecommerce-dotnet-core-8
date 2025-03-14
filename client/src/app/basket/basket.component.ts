import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderSummaryComponent } from '../shared/order-summary/order-summary.component';
import { IBasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  imports: [CommonModule,
    RouterLink,
    OrderSummaryComponent
  ],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent {
  
  constructor(public basketService: BasketService) { }

  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

}