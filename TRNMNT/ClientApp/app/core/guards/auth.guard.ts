import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service';

/**
 * Decides if a route can be activated.
 */
@Injectable()
export class AuthGuard implements CanActivate {
  constructor(public authService: AuthService, private routerService: RouterService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const expectedRoles: string[] = route.data.expectedRoles;
    const url: string = state.url;
    if (this.authService.isLoggedIn()) {
      if (expectedRoles) {
        const role = this.authService.getUser().role;
        if (expectedRoles.indexOf(role) === -1) {
          this.authService.redirectUrl = url;
          // Not signed in so redirects to signin page.
          this.routerService.goToLogin(url);
          return false;
        }
      }
      return true;
    } else {
      // Stores the attempted URL for redirecting.
      this.authService.redirectUrl = url;
      // Not signed in so redirects to signin page.
      this.routerService.goToLogin(url);
      return false;
    }
  }
}
