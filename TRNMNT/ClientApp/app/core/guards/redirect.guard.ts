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

  getSubdomains(): string[] {
    const parts = window.location.host.split('.');
    parts.pop();
    if (parts.filter(p => p.indexOf('trnmnt') !== -1).length > 0) {
      parts.pop();
    }
    return parts;
  }

  logParts(parts) {
    for (var part in parts) {
      console.log(part)
    }
  }

  redirectToEventPage(): boolean {
    return this.getSubdomains().length === 1 && !this.routerService.getCurrentUrl().startsWith('/event/');
  }
}
