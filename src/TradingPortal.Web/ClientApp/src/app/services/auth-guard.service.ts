

import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild, NavigationExtras, CanLoad, Route } from '@angular/router';
import { AuthService } from './auth.service';
import {AccountService} from './account.service';


@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private authService: AuthService, private accountService: AccountService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    let url: string = state.url;
    if (this.checkLogin(url)) {
      if (this.authService.isPortalClosed) {
        this.authService.storeCloseDelegate(true);
        if ((url.indexOf('storeclose') > -1) || (url.indexOf('adminsection') > -1 && this.accountService.isAdminUser)) {
          return true;
        }
        else {
          this.router.navigate(['/storeclose']);
          return false;
        }
      }
      else if (url.indexOf('storeclose') > -1) {
        this.router.navigate(['/']);
        return false;
      }
      else if (url.indexOf('adminsection') > -1 && !this.accountService.isAdminUser) {
        return false;
      }
      return true;
    }
    return false;
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }

  canLoad(route: Route): boolean {

    let url = `/${route.path}`;
    return this.checkLogin(url);
  }

  checkLogin(url: string): boolean {

    if (this.authService.isLoggedIn) {
      return true;
    }
    else {
      this.authService.loginRedirectUrl = url;
      this.router.navigate(['/login']);

      return false;
    }
  }
}
