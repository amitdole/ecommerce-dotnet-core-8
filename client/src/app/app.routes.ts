import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './store/product-details/product-details.component';
import { StoreComponent } from './store/store.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { UnAuthenticatedComponent } from './core/un-authenticated/un-authenticated.component';
import { BasketComponent } from './basket/basket.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { CheckoutComponent } from './checkout/checkout/checkout.component';
import { SigninRedirectCallbackComponent } from './account/signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './account/signout-redirect-callback/signout-redirect-callback.component';
import { MsalGuard } from '@azure/msal-angular';

export const routes: Routes = [
    {path: '', component:HomeComponent, data: {breadcrumb: 'Home'}},    
    {path: 'login', component:LoginComponent , data: {breadcrumb: {skip: true}}},    
    {path: 'register', component:RegisterComponent , data: {breadcrumb: {skip: true}}},    
    {path: 'store', component:StoreComponent , data: {breadcrumb: 'Store'}},
    {path: 'signin-callback', component:SigninRedirectCallbackComponent },
    {path: 'signout-callback', component:SignoutRedirectCallbackComponent},
    {path: 'store/:id', component:ProductDetailsComponent , data: {breadcrumb: {alias: 'productDetails'}}}, 
    {path: 'basket', component:BasketComponent , data: {breadcrumb: 'Basket'}},    
    {path: 'checkout', component:CheckoutComponent , data: {breadcrumb: 'Checkout'}},    
    {path: '**', redirectTo: '', pathMatch: 'full'},
    {path: 'server-error', component:ServerErrorComponent},
    {path: 'not-found', component:NotFoundComponent},
    {path: 'un-authenticated', component:UnAuthenticatedComponent}
];
