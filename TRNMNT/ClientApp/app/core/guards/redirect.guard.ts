import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { RouterService } from '../services/router.service';

@Injectable()
export class RedirectGuard implements CanActivate {
  constructor(private routerService: RouterService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.redirectToEventPage()) {
      this.routerService.goToEventInfo();
    } else {
      return true;
    }
  }
  redirectToEventPage(): boolean {
    return this.routerService.getSubdomains().length === 1 && !this.routerService.getCurrentUrl().startsWith('/event/');
  }
}
