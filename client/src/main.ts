import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter} from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { MsalGuardConfiguration, MsalInterceptorConfiguration, MsalModule } from '@azure/msal-angular';
import { BrowserCacheLocation, InteractionType, PublicClientApplication } from '@azure/msal-browser';

export function MSALInstanceFactory(): PublicClientApplication {
  return new PublicClientApplication({
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
}

//MSAL guard config
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: ['openid', 'profile', 'https://sportscenter29.onmicrosoft.com/fc93cce5-e708-4e2b-9b8b-20f96db8abf3/access_as_user']
    }
  };
}

//MSAL interceptor config
export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: new Map([
      ['https://sportscenter29.onmicrosoft.com/fc93cce5-e708-4e2b-9b8b-20f96db8abf3',['https://sportscenter29.onmicrosoft.com/fc93cce5-e708-4e2b-9b8b-20f96db8abf3/access_as_user']]
  ])
  };
}


bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(), // <-- Add this
    provideRouter(routes),
    importProvidersFrom(
       MsalModule.forRoot(
            MSALInstanceFactory(),
            MSALGuardConfigFactory(),
            MSALInterceptorConfigFactory()
          )
    )
  ]
}).catch(err => console.error(err));