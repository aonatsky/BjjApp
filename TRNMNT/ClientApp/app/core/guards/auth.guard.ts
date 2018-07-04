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
    if (this.authService.isLoggedIn()) {
      // Signed in.
      return true;
    }
    // Stores the attempted URL for redirecting.
    const url: string = state.url;
    this.authService.redirectUrl = url;
    // Not signed in so redirects to signin page.
    this.routerService.goToLogin(url);
    return false;
  }
}
