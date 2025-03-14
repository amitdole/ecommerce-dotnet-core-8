import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { BehaviorSubject } from 'rxjs';
import { IProduct } from '../shared/models/product';
import { AccountService } from '../account/account.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl ="http://localhost:8010";
  constructor(private http: HttpClient,private acntService: AccountService, private router: Router) { }

  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  basketTotal$ = this.basketTotal.asObservable();

  getBasket(userName: string){
    return this.http.get<IBasket>(this.baseUrl + '/Basket/GetBasket/amit' + userName).subscribe((basket: IBasket) => {
      this.basketSource.next(basket);
      this.calculateBasketTotal();
    }, error => {
      console.log(error);
    });
  }

  setBasket(basket: IBasket){
    return this.http.post<IBasket>(this.baseUrl + '/Basket/CreateBasket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateBasketTotal();
    }, error => {
      console.log(error);
    });
  }

  checkoutBasket(basket: IBasket){
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': this.acntService.authorizationHeaderValue
      })
    };
    return this.http.post<IBasket>(this.baseUrl +'/Basket/CheckoutV2', basket, httpOptions).subscribe({
      next: basket =>{
        this.basketSource.next(null);
        this.router.navigateByUrl('/');
      }
    });
  }

  getCurrentBasket(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const item = items.find(i => i.productId === itemToAdd.productId);
    if(item){
      item.quantity += quantity;
    }else{
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_userName', 'amit');
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct):IBasketItem{
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      imageFile: item.imageFile
  }
}

  private calculateBasketTotal(){
    const basket = this.getCurrentBasket();
    if(basket){
      const total = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
      this.basketTotal.next({total});
        }
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if (!basket) return;
    const foundItemIndex = basket.items.findIndex(x => x.productId === item.productId);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  } 

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if (!basket) return;
    const foundItemIndex = basket.items.findIndex(x => x.productId === item.productId);
    if(basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }else{
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if (!basket) return;
    if (basket.items.some(x => x.productId === item.productId)) {
      basket.items = basket.items.filter(i => i.productId !== item.productId);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + '/Basket/DeleteBasket/' + basket.userName).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotal.next(null);
      localStorage.removeItem('basket_userName');
    }, error => {
      console.log(error);
    });
  }
  
}