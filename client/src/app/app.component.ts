import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from './core/navbar/navbar.component'; 
import { CommonModule } from '@angular/common';
import { IProduct } from './shared/models/product';
import { Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './core/header/header.component';
import { NgxSpinner, NgxSpinnerComponent } from 'ngx-spinner';
import { BasketService } from './basket/basket.service';
import { RegisterComponent } from './account/register/register.component';
import { LoginComponent } from './account/login/login.component';
import { MsalGuardConfiguration, MsalInterceptorConfiguration, MsalModule, MsalService } from '@azure/msal-angular';
import { BrowserCacheLocation, InteractionType, PublicClientApplication } from '@azure/msal-browser';
import { AccountService } from './account/account.service';


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

  constructor(
    private basketService:BasketService, 
    private msalService: MsalService, 
    private accountService: AccountService,
    private router: Router
    ) {

  }
  
  async ngOnInit(): Promise<void> {
   
    await this.initializeMsal();

    this.msalService.instance.handleRedirectPromise().then((result) => {
      if(result && result.account){
        console.log('login success');

        const targetRoute = result.state || '/';
        console.log('Navigating to:', targetRoute);
        this.router.navigate([targetRoute]);
        //set the active account and update the user state
        this.msalService.instance.setActiveAccount(result.account);
        this.accountService.SetUserAfterRedirect();
      }
    }).catch((error) => {
      console.error(error);
    });

    const basket_username = localStorage.getItem('basket_userName');
    if(basket_username){
      this.basketService.getBasket(basket_username);
    }

  }

  private async initializeMsal(): Promise<void> {
    const pca =  new PublicClientApplication({
      auth: {
        clientId: 'fc93cce5-e708-4e2b-9b8b-20f96db8abf3',
        authority: 'https://sportscenter29.b2clogin.com/sportscenter29.onmicrosoft.com/B2C_1_SignInSignUp/v2.0/',
        redirectUri: 'http://localhost:4200',
        knownAuthorities: ['sportscenter29.b2clogin.com'],    
      },
      cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage,
        storeAuthStateInCookie: false
    }});
    this.msalService.instance = pca;
    await pca.initialize();
  }
}
