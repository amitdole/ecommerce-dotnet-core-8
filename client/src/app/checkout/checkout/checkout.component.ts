import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { AccountService } from '../../account/account.service';
import { IBasket, IBasketItem } from '../../shared/models/basket';
import { RouterLink } from '@angular/router';
import { OrderSummaryComponent } from '../../shared/order-summary/order-summary.component';

@Component({
  selector: 'app-checkout',
  imports: [CommonModule,
    RouterLink,
    OrderSummaryComponent
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {
  constructor(public basketService: BasketService, private acntService: AccountService){}

  ngOnInit(): void {
    this.acntService.currentUser$.subscribe({
      next:(res) =>{
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },error:(err) =>{
        console.log(`An error occurred while setting isUserAuthenticated flag.`)
      }
    })
  }
  public isUserAuthenticated: boolean = false;


  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: IBasketItem){
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem){
    this.basketService.decrementItemQuantity(item);
  }

  orderNow(item: IBasket){
    this.basketService.checkoutBasket(item);
  }
}