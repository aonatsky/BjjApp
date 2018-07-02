import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AuthGuard } from '../routing/auth.guard';
import { RouterService } from '../services/router.service'


/** 
 * Decides if a route can be activated. 
 */
@Injectable() export class RedirectGuard implements CanActivate {

    constructor(public authService: AuthService, private routerService: RouterService, private authGuard: AuthGuard) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.redirectToEventPage()) {
            this.routerService.goToEventInfo();
        }
        else {
            return true;
        };
    }

    getSubdomains(): string[] {
        const parts = window.location.host.split('.');
        parts.pop();
        return parts;
    }

    redirectToEventPage(): boolean {
        return this.getSubdomains().length === 1 && !this.routerService.getCurrentUrl().startsWith('/event/');
    }

}  