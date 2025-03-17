import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReplaySubject } from 'rxjs';

import { Router } from '@angular/router';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { Constants } from './constants';
import { MsalService } from '@azure/msal-angular';
import { AuthenticationResult } from '@azure/msal-browser';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // We need to have something which won't emit initial value rather wait till it has something.
  // Hence for that ReplaySubject. I have given to hold one user object and it will cache this as well
  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();

  //identity server
  private manager = new UserManager(getClientSettings());
  private user!: User | null;
  // token = "";
  // access_token = "";

  constructor(private http: HttpClient, private router: Router, private msalService: MsalService) {
    // this.manager.getUser().then(user => {
    //   this.user = user;
    //   this.currentUserSource.next(this.isAuthenticated());
    // });
    //Check if user is authenticated and set user state accordingly
    if (this.msalService.instance.getActiveAccount()){
      this.currentUserSource.next(this.msalService.instance.getActiveAccount());
    }
  }

  isAuthenticated(): boolean {
    // return this.user != null && !this.user.expired;
    return this.msalService.instance.getActiveAccount() !== null;
  }

  login(state?: string) {
    //return this.manager.signinRedirect();
    //const state = this.router.url;
    this.msalService.loginRedirect({
      scopes: ['openid', 'profile', 'https://sportscenter29.onmicrosoft.com/fc93cce5-e708-4e2b-9b8b-20f96db8abf3'],
      state: state
    });
  }

  async signout() {
    await this.manager.signoutRedirect();
  }

  SetUserAfterRedirect():void{
    const account = this.msalService.instance.getActiveAccount();
    console.log('account', account);
    if(account){
      this.currentUserSource.next(account);
    }else{
      this.currentUserSource.next(null);
    } 
  }

  get authorizationHeaderValue(): Promise<string> {
    const account = this.msalService.instance.getActiveAccount();
    if (account) {
       return this.msalService.instance.acquireTokenSilent({
        scopes: ['openid', 'profile'],
        account: account
        }).then((results: AuthenticationResult)=> {
          return `${results.tokenType} ${results.accessToken}`;
        });
    }
    return Promise.resolve('');
  }

  logout() {
    //localStorage.removeItem('token');
    this.msalService.logoutRedirect({
      postLogoutRedirectUri: 'http://localhost:4200'
    });
    this.currentUserSource.next(null);
    //this.router.navigateByUrl('/');
  }

  
  // public finishLogin = (): Promise<User> => {
  //   return this.manager.signinRedirectCallback()
  //   .then(user => {
  //     this.currentUserSource.next(this.checkUser(user));
  //     this.token = user.token_type;
  //     this.access_token = user.access_token;
  //     return user;
  //   })
  // }

  public finishLogout = () => {
    this.user = null;
    return this.manager.signoutRedirectCallback();
  }

  private checkUser = (user : User): boolean => {
    console.log('inside check user');
    console.log(user);
    return !!user && !user.expired;
  }



}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`,
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: `${Constants.clientRoot}/signin-callback`,
    scope: "openid profile eshoppinggateway",
    response_type: "code",
    post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
  };
}