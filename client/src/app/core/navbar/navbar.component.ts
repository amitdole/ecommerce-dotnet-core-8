import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BasketService } from '../../basket/basket.service';
import { CommonModule } from '@angular/common';
import { IBasketItem } from '../../shared/models/basket';
import { AccountService } from '../../account/account.service';

@Component({
  selector: 'app-navbar',
  imports: [
    CommonModule,
    RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit  {
  constructor(public basketService: BasketService, private acntService: AccountService) { 

    
  }
  ngOnInit(): void {
    console.log(`current user:`);
    this.acntService.currentUser$.subscribe({
      next:(user) =>{
        this.isUserAuthenticated = !!user;
        console.log(this.isUserAuthenticated);
      },error:(err) =>{
        console.log(`An error occurred while setting isUserAuthenticated flag.`)
      }
    })
  }

  public isUserAuthenticated: boolean = false;
  
  public login = () => {
    this.acntService.login();
  }
  public logout = () => {
    this.acntService.signout();
  }
  getBasketCount(items: IBasketItem[]) {
    return items.reduce((a, b) => b.quantity + a, 0);
  }

}
