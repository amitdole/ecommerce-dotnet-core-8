import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map } from 'rxjs';
import { Observable } from 'rxjs';
import { AccountService } from '../../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router,private acntService: AccountService) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.acntService.currentUser$.pipe(
        map(auth =>{
        if(auth) return true;
        else{
          this.router.navigate(['/account/login'], {queryParams: {returnUrl: state.url}, replaceUrl: true });
          return false;
        }
      })
    )
  }

}
