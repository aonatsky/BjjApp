import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AuthGuard } from './auth.guard';
import { RouterService } from '../services/router.service';

@Injectable()
export class RedirectGuard implements CanActivate {
  constructor(public authService: AuthService, private routerService: RouterService, private authGuard: AuthGuard) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.redirectToEventPage()) {
      this.authService.goToHomePage = this.routerService.goToEventInfo;
      this.routerService.goToEventInfo();
    } else {
      this.authService.goToHomePage = this.routerService.goToLogin;
      return true;
    }
  }
  redirectToEventPage(): boolean {
    return this.routerService.getSubdomains().length === 1 && !this.routerService.getCurrentUrl().startsWith('/event/');
  }
}
