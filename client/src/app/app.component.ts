import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from './core/navbar/navbar.component'; 
import { CommonModule } from '@angular/common';
import { IProduct } from './shared/models/product';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './core/header/header.component';
import { NgxSpinner, NgxSpinnerComponent } from 'ngx-spinner';
import { BasketService } from './basket/basket.service';
import { RegisterComponent } from './account/register/register.component';
import { LoginComponent } from './account/login/login.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [
    NavbarComponent,
    HeaderComponent,
    CommonModule,
    RouterOutlet,
    NgxSpinnerComponent
  ]
})
export class AppComponent implements OnInit {
  title = 'eShopping';
  products: IProduct[]  = [];

  constructor(private basketService:BasketService) {
    const baskket_username = localStorage.getItem('basket_userName');
    if(baskket_username){
      this.basketService.getBasket(baskket_username);
    }
    console.log('AppComponent constructor');
  }
  
  ngOnInit(): void {
   
  }
}
